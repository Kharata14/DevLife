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

namespace DevLife.Application.Features.CodeCasino.Queries;
public class GetDailyChallengeQueryHandler : IRequestHandler<GetDailyChallengeQuery, PublicCasinoChallengeDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly ICasinoChallengeGenerator _challengeGenerator;
    private readonly IDistributedCache _cache;

    public GetDailyChallengeQueryHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, ICasinoChallengeGenerator challengeGenerator, IDistributedCache cache)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _challengeGenerator = challengeGenerator;
        _cache = cache;
    }

    public async Task<PublicCasinoChallengeDto> Handle(GetDailyChallengeQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var dateKey = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var userPlayedCacheKey = $"daily_played:{userId}:{dateKey}";

        if (await _cache.GetStringAsync(userPlayedCacheKey, cancellationToken) != null)
        {
            throw new InvalidOperationException("You have already completed the daily challenge today. Come back tomorrow!");
        }

        var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
        var fullChallenge = await _challengeGenerator.GenerateChallengeAsync(user.TechStack, user.ExperienceLevel);

        var challengeId = Guid.NewGuid();
        var challengeToCache = new CachedCasinoChallengeDto
        {
            ChallengeId = challengeId,
            Description = fullChallenge.Description,
            Snippets = fullChallenge.Snippets
        };
        var userChallengeCacheKey = $"casino:challenge:{userId}";
        var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) };
        await _cache.SetStringAsync(userChallengeCacheKey, JsonSerializer.Serialize(challengeToCache), cacheOptions, cancellationToken);

        return new PublicCasinoChallengeDto
        {
            ChallengeId = challengeId,
            Description = fullChallenge.Description,
            Snippets = fullChallenge.Snippets.Select(s => new PublicCodeSnippetDto { Id = s.Id, Code = s.Code }).ToList()
        };
    }
}
