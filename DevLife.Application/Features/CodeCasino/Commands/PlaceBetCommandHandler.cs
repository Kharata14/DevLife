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
public class PlaceBetCommandHandler : IRequestHandler<PlaceBetCommand, PlaceBetResultDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IDistributedCache _cache;
    // private readonly ICasinoGameRepository _gameLogRepository;

    public PlaceBetCommandHandler(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IDistributedCache cache)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _cache = cache;
    }

    public async Task<PlaceBetResultDto> Handle(PlaceBetCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
        if (user.Points < request.BetAmount)
        {
            throw new InvalidOperationException("Insufficient points to place this bet.");
        }

        var cacheKey = $"casino:challenge:{userId}";
        var cachedJson = await _cache.GetStringAsync(cacheKey, cancellationToken);
        if (string.IsNullOrEmpty(cachedJson))
        {
            throw new InvalidOperationException("Challenge has expired or does not exist. Please get a new one.");
        }
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        var challenge = JsonSerializer.Deserialize<CachedCasinoChallengeDto>(cachedJson);

        if (challenge.ChallengeId != request.ChallengeId)
        {
            throw new InvalidOperationException("Invalid challenge ID.");
        }

        var correctSnippet = challenge.Snippets.First(s => s.IsCorrect);
        bool wasCorrect = request.ChosenSnippetId == correctSnippet.Id;
        int pointsChange = wasCorrect ? request.BetAmount : -request.BetAmount;

        await _userRepository.UpdateUserPointsAsync(userId, pointsChange);

        return new PlaceBetResultDto
        {
            WasCorrect = wasCorrect,
            Explanation = correctSnippet.Explanation,
            PointsChange = pointsChange,
            NewTotalPoints = user.Points + pointsChange
        };
    }
}
