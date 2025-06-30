using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DevLife.Application.Features.Dating.Queries
{
    public class GetPotentialMatchesQueryHandler : IRequestHandler<GetPotentialMatchesQuery, List<DatingProfileDto>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IUserSwipeRepository _swipeRepository;
        private readonly IDatingProfileRepository _datingProfileRepository;

        public GetPotentialMatchesQueryHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IUserSwipeRepository swipeRepository, IDatingProfileRepository datingProfileRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _swipeRepository = swipeRepository;
            _datingProfileRepository = datingProfileRepository;
        }

        public async Task<List<DatingProfileDto>> Handle(GetPotentialMatchesQuery request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user.InterestedIn == null || user.Gender == null)
            {
                throw new InvalidOperationException("User must set their gender and dating preferences first.");
            }

            var swipedIds = await _swipeRepository.GetSwipedProfileIdsAsync(userId);

            var matches = await _datingProfileRepository.GetPotentialMatchesAsync(user, swipedIds);

            return matches.Select(p => new DatingProfileDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Bio = p.Bio,
                ProfileImageUrl = p.ProfileImageUrl,
                TechStack = p.TechStack,
                CompatibilityScore = p.TechStack.Intersect(user.TechStack.Split(',').Select(t => t.Trim())).Count()
            }).ToList();
        }
    }
}