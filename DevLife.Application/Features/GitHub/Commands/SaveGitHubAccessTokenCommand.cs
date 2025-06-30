using MediatR;
namespace DevLife.Application.Features.GitHub.Commands
{
    public class SaveGitHubAccessTokenCommand : IRequest
    {
        public string TemporaryCode { get; set; }
    }
}