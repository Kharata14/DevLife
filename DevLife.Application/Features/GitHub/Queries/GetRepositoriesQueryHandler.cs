using DevLife.Application.Features.GitHub.Dtos;
using DevLife.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.GitHub.Queries
{
    public class GetRepositoriesQueryHandler : IRequestHandler<GetRepositoriesQuery, List<GitHubRepoDto>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IGitHubService _githubService;

        public GetRepositoriesQueryHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IGitHubService githubService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _githubService = githubService;
        }

        public async Task<List<GitHubRepoDto>> Handle(GetRepositoriesQuery request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);

            if (string.IsNullOrEmpty(user.GitHubAccessToken))
            {
                throw new InvalidOperationException("User has not linked their GitHub account.");
            }

            return await _githubService.GetUserRepositoriesAsync(user.GitHubAccessToken);
        }
    }
}
