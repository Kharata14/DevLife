using DevLife.Application.Features.GitHub.Analysis.Commands;
using DevLife.Application.Features.GitHub.Analysis.Queries;
using DevLife.Application.Features.GitHub.Commands;
using DevLife.Application.Features.GitHub.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevLife.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GitHubController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GitHubController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("github-login")]
        public async Task<IActionResult> Login()
        {
            var query = new GetGitHubAuthUrlQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("GitHub did not provide an authorization code.");
            }

            var command = new SaveGitHubAccessTokenCommand { TemporaryCode = code };
            await _mediator.Send(command);

            return Ok("GitHub account linked successfully! You can now close this tab.");
        }

        [HttpGet("repos")]
        public async Task<IActionResult> GetRepositories()
        {
            var query = new GetRepositoriesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> StartAnalysis([FromBody] StartAnalysisCommand command)
        {
            var jobId = await _mediator.Send(command);
            return AcceptedAtAction(nameof(GetAnalysisResult), new { jobId = jobId }, new { jobId = jobId });
        }

        [HttpGet("analyze/{jobId:guid}")]
        public async Task<IActionResult> GetAnalysisResult(Guid jobId)
        {
            var query = new GetAnalysisResultQuery { JobId = jobId };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new { message = "Analysis job not found." });
            }

            return Ok(result);
        }

        [HttpGet("analyze/{jobId:guid}/card")]
        public async Task<IActionResult> GetAnalysisCard(Guid jobId)
        {
            var query = new GetAnalysisCardQuery { JobId = jobId };
            var imageBytes = await _mediator.Send(query);
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return NotFound("Could not generate or retrieve card image.");
            }
            return File(imageBytes, "image/png");
        }
    }
}