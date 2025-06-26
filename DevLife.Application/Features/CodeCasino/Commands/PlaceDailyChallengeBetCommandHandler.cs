using DevLife.Application.Features.CodeCasino.Dtos;
using DevLife.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevLife.Application.Features.CodeCasino.Commands;
public class PlaceDailyChallengeBetCommandHandler : IRequestHandler<PlaceDailyChallengeBetCommand, PlaceBetResultDto>
{
    private const int DailyChallengeReward = 250;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDistributedCache _cache;
    private readonly IUserRepository _userRepository;

    public PlaceDailyChallengeBetCommandHandler(IHttpContextAccessor httpContextAccessor, IDistributedCache cache, IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _cache = cache;
        _userRepository = userRepository;
    }

    public async Task<PlaceBetResultDto> Handle(PlaceDailyChallengeBetCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var dateKey = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var userPlayedCacheKey = $"daily_played:{userId}:{dateKey}";

        if (await _cache.GetStringAsync(userPlayedCacheKey, cancellationToken) != null)
        {
            throw new InvalidOperationException("You have already completed the daily challenge today.");
        }

        var userChallengeCacheKey = $"casino:challenge:{userId}";
        var cachedJson = await _cache.GetStringAsync(userChallengeCacheKey, cancellationToken);
        if (string.IsNullOrEmpty(cachedJson))
        {
            throw new InvalidOperationException("Daily challenge has expired. Please get a new one.");
        }

        await _cache.RemoveAsync(userChallengeCacheKey, cancellationToken);
        var endOfDay = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);
        await _cache.SetStringAsync(userPlayedCacheKey, "completed", new DistributedCacheEntryOptions { AbsoluteExpiration = endOfDay }, cancellationToken);

        var challenge = JsonSerializer.Deserialize<CachedCasinoChallengeDto>(cachedJson);
        if (challenge.ChallengeId != request.ChallengeId)
        {
            throw new InvalidOperationException("Invalid daily challenge ID.");
        }

        var correctSnippet = challenge.Snippets.First(s => s.IsCorrect);
        bool wasCorrect = request.ChosenSnippetId == correctSnippet.Id;
        int pointsChange = wasCorrect ? DailyChallengeReward : 0; 

        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
        if (pointsChange > 0)
        {
            await _userRepository.UpdateUserPointsAsync(userId, pointsChange);
        }

        return new PlaceBetResultDto
        {
            WasCorrect = wasCorrect,
            Explanation = correctSnippet.Explanation,
            PointsChange = pointsChange,
            NewTotalPoints = user.Points + pointsChange
        };
    }
}
