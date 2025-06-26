// In: Infrastructure/EventHandlers/SolutionSubmittedEventHandler.cs
using DevLife.Application.Features.CodeRoast.Dtos;
using DevLife.Application.Features.CodeRoast.Notifications;
using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using DevLife.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Caching.Distributed; // <-- დაგვჭირდება Redis-ისთვის
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.EventHandlers
{
    public class SolutionSubmittedEventHandler : INotificationHandler<SolutionSubmittedEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<SolutionSubmittedEventHandler> _logger;

        public SolutionSubmittedEventHandler(IServiceScopeFactory serviceScopeFactory, ILogger<SolutionSubmittedEventHandler> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public Task Handle(SolutionSubmittedEvent notification, CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var executionService = scope.ServiceProvider.GetRequiredService<ICodeExecutionService>();
                var roastService = scope.ServiceProvider.GetRequiredService<IAiCodeRoastService>();
                var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
                var challengeCacheKey = $"roast_challenge:{notification.ChallengeId}";
                var challengeJson = await cache.GetStringAsync(challengeCacheKey, cancellationToken);

                if (string.IsNullOrEmpty(challengeJson))
                {
                    _logger.LogWarning("Could not find challenge details in cache for ChallengeId: {ChallengeId}. Aborting roast.", notification.ChallengeId);
                    return;
                }

                var challenge = JsonSerializer.Deserialize<CodingChallengeDto>(challengeJson);

                await cache.RemoveAsync(challengeCacheKey, cancellationToken);
                var submission = new CodeRoastSubmission
                {
                    Id = notification.SubmissionId,
                    UserId = notification.UserId,
                    ChallengeId = notification.ChallengeId,
                    SubmittedCode = notification.SourceCode,
                    Status = "Pending"
                };
                await dbContext.CodeRoastSubmissions.AddAsync(submission, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                try
                {
                    submission.Status = "Executing";
                    await dbContext.SaveChangesAsync(cancellationToken);
                    CodeExecutionResultDto executionResult = await executionService.ExecuteCodeAsync(submission.SubmittedCode, challenge.Language);

                    submission.ExecutionOutput = executionResult.IsSuccess ? executionResult.Output : executionResult.Error;
                    submission.Status = "Roasting";
                    await dbContext.SaveChangesAsync(cancellationToken);

                    string roast = await roastService.GetRoastAsync(challenge.Description, submission.SubmittedCode, executionResult);
                    submission.AiRoast = roast;
                    submission.Status = "Completed";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Processing submission {SubmissionId} failed.", notification.SubmissionId);
                    submission.Status = "Failed";
                }
                finally
                {
                    submission.CompletedAt = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync(cancellationToken);
                }

            }, cancellationToken);

            return Task.CompletedTask;
        }
    }
}