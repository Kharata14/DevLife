using DevLife.Application.Features.GitHub.Dtos;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevLife.Application.Features.GitHub.Queries
{
    public class GetGitHubAuthUrlQueryHandler : IRequestHandler<GetGitHubAuthUrlQuery, GitHubLoginResponseDto>
    {
        private readonly IConfiguration _configuration;

        public GetGitHubAuthUrlQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<GitHubLoginResponseDto> Handle(GetGitHubAuthUrlQuery request, CancellationToken cancellationToken)
        {
            var clientId = _configuration["GitHub:ClientId"];
            if (string.IsNullOrEmpty(clientId))
            {
                throw new InvalidOperationException("GitHub ClientId is not configured in User Secrets.");
            }

            var authorizationUrl = $"https://github.com/login/oauth/authorize?client_id={clientId}&scope=repo,user";

            var response = new GitHubLoginResponseDto
            {
                AuthorizationUrl = authorizationUrl
            };
            return Task.FromResult(response);
        }
    }
}