using DevLife.Application.Features.GitHub.Analysis.Notifications;
using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using DevLife.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.EventHandlers
{
    public class AnalysisStartedEventHandler : INotificationHandler<AnalysisStartedEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<AnalysisStartedEventHandler> _logger;

        public AnalysisStartedEventHandler(IServiceScopeFactory serviceScopeFactory, ILogger<AnalysisStartedEventHandler> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public Task Handle(AnalysisStartedEvent notification, CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var analysisService = scope.ServiceProvider.GetRequiredService<IGitHubAnalysisService>();
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                var job = new GitHubAnalysisJob
                {
                    Id = notification.JobId,
                    UserId = notification.UserId,
                    RepositoryOwner = notification.Owner,
                    RepositoryName = notification.RepoName,
                    Status = "Pending"
                };
                await dbContext.GitHubAnalysisJobs.AddAsync(job);
                await dbContext.SaveChangesAsync();

                try
                {
                    job.Status = "Analyzing";
                    await dbContext.SaveChangesAsync();

                    var user = await userRepository.GetUserByIdAsync(notification.UserId);
                    var analysisResultJson = await analysisService.AnalyzeRepositoryAsync(notification.Owner, notification.RepoName, user.GitHubAccessToken);

                    job.ResultJson = analysisResultJson;
                    job.Status = "Completed";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Processing GitHub analysis job {JobId} failed.", notification.JobId);
                    job.Status = "Failed";
                }
                finally
                {
                    job.CompletedAt = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync();
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }
    }
}