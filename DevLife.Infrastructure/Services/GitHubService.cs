using DevLife.Application.Features.GitHub.Dtos;
using DevLife.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public GitHubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<string> GetAccessTokenAsync(string temporaryCode)
        {
            var clientId = _configuration["GitHub:ClientId"];
            var clientSecret = _configuration["GitHub:ClientSecret"];
            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token")
            {
                Headers = { { "Accept", "application/json" } },
                Content = JsonContent.Create(new { client_id = clientId, client_secret = clientSecret, code = temporaryCode })
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadFromJsonAsync<GitHubTokenResponse>();
            return responseData?.access_token;
        }

        public async Task<List<GitHubRepoDto>> GetUserRepositoriesAsync(string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Add("User-Agent", "DevLife-Portal-App");

            var graphQLQuery = new
            {
                query = @"
                    query {
                      viewer {
                        repositories(first: 50, privacy: PUBLIC, orderBy: {field: PUSHED_AT, direction: DESC}, ownerAffiliations: OWNER) {
                          nodes { name, owner { login }, description, url, primaryLanguage { name } }
                        }
                      }
                    }"
            };

            var response = await client.PostAsJsonAsync("https://api.github.com/graphql", graphQLQuery);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadFromJsonAsync<GitHubReposResponse>();

            return responseData.Dt.Viewer.Repositories.Nodes
                .Select(repo => new GitHubRepoDto
                {
                    Name = repo.Name,
                    Owner = repo.Owner.Login,
                    Description = repo.Description,
                    Url = repo.Url,
                    PrimaryLanguage = repo.PrimaryLanguage?.Name ?? "N/A"
                }).ToList();
        }
        private class GitHubTokenResponse
        {
            [JsonPropertyName("access_token")]
            public string access_token { get; set; }
        }

        private class GitHubReposResponse
        {
            [JsonPropertyName("data")]
            public ResponseData Dt { get; set; }
        }
        private class ResponseData { public Viewer Viewer { get; set; } }
        private class Viewer { public Repositories Repositories { get; set; } }
        private class Repositories { public List<RepoNode> Nodes { get; set; } }
        private class RepoNode { public string Name { get; set; } public Owner Owner { get; set; } public string Description { get; set; } public string Url { get; set; } public PrimaryLanguage PrimaryLanguage { get; set; } }
        private class Owner { public string Login { get; set; } }
        private class PrimaryLanguage { public string Name { get; set; } }
    }
}