using DevLife.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DevLife.Application.Features.GitHub.Commands
{
    public class SaveGitHubAccessTokenCommandHandler : IRequestHandler<SaveGitHubAccessTokenCommand>
    {
        private readonly IGitHubService _githubService;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SaveGitHubAccessTokenCommandHandler(IGitHubService githubService, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _githubService = githubService;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(SaveGitHubAccessTokenCommand request, CancellationToken cancellationToken)
        {
            var accessToken = await _githubService.GetAccessTokenAsync(request.TemporaryCode);
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new InvalidOperationException("Could not retrieve GitHub access token.");
            }
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
            user.GitHubAccessToken = accessToken;
            await _userRepository.UpdateUserAsync(user);
        }
    }
}