using DevLife.Application.Features.CodeRoast.Dtos;
using DevLife.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DevLife.Application.Features.CodeRoast.Queries
{
    public class GetRoastChallengeQueryHandler : IRequestHandler<GetRoastChallengeQuery, CodingChallengeDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly ICodingChallengeProvider _challengeProvider;
        private readonly IDistributedCache _cache;

        public GetRoastChallengeQueryHandler(
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            ICodingChallengeProvider challengeProvider,
            IDistributedCache cache)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _challengeProvider = challengeProvider;
            _cache = cache;
        }

        public async Task<CodingChallengeDto> Handle(GetRoastChallengeQuery request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);

            var challenge = await _challengeProvider.GetChallengeAsync(user.TechStack, request.Difficulty);

            var cacheKey = $"roast_challenge:{challenge.Id}";
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(challenge), cacheOptions, cancellationToken);
            return challenge;
        }
    }
}