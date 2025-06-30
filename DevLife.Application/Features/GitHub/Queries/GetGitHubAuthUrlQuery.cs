using DevLife.Application.Features.GitHub.Dtos;
using MediatR;

namespace DevLife.Application.Features.GitHub.Queries
{
    public class GetGitHubAuthUrlQuery : IRequest<GitHubLoginResponseDto>
    {
    }
}