using DevLife.Application.Features.GitHub.Dtos;
using DevLife.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Services
{
    public class GitHubAnalysisService : IGitHubAnalysisService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OpenAIClient _openAiClient;
        private readonly ILogger<GitHubAnalysisService> _logger;
        private readonly IConfiguration _configuration;

        public GitHubAnalysisService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<GitHubAnalysisService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
            var apiKey = _configuration["ApiKeys:OpenAiApiKey"];
            _openAiClient = !string.IsNullOrEmpty(apiKey) ? new OpenAIClient(apiKey) : null;
        }

        public async Task<string> AnalyzeRepositoryAsync(string owner, string repoName, string accessToken)
        {
            if (_openAiClient == null) throw new InvalidOperationException("OpenAI client is not configured.");

            var githubData = await GetGitHubDataAsync(owner, repoName, accessToken);
            var summary = SummarizeGitHubData(githubData);
            _logger.LogInformation("Generated GitHub summary for {Owner}/{RepoName}: {Summary}", owner, repoName, summary);
            return await GetPersonaFromAiAsync(summary);
        }

        private async Task<GitHubAnalysisResponse> GetGitHubDataAsync(string owner, string repoName, string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Add("User-Agent", "DevLife-Portal-App");

            var graphQLQuery = new
            {
                query = $@"
                    query {{
                      repository(owner: ""{owner}"", name: ""{repoName}"") {{
                        defaultBranchRef {{
                          target {{
                            ... on Commit {{
                              history(first: 30) {{
                                nodes {{
                                  message
                                }}
                              }}
                            }}
                          }}
                        }}
                        object(expression: ""HEAD:"") {{
                          ... on Tree {{
                            entries {{
                              name
                              type
                            }}
                          }}
                        }}
                      }}
                    }}"
            };

            var response = await client.PostAsJsonAsync("https://api.github.com/graphql", graphQLQuery);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadFromJsonAsync<GitHubAnalysisResponse>();
            return responseData;
        }

        private string SummarizeGitHubData(GitHubAnalysisResponse data)
        {
            try
            {
                var commitMessages = data.Data.Repository.DefaultBranchRef.Target.History.Nodes.Select(c => c.Message).ToList();
                var files = data.Data.Repository.Object.Entries;

                int fileCount = files.Count(e => e.Type == "blob");
                int folderCount = files.Count(e => e.Type == "tree");
                double avgCommitLength = commitMessages.Any() ? commitMessages.Average(m => m.Length) : 0;

                return $"Repository analysis summary: File count in root directory is {fileCount}. Folder count is {folderCount}. Average commit message length is {avgCommitLength:F1} characters. Last 5 commit messages are: [{string.Join(", ", commitMessages.Take(5).Select(m => $"'{m.Replace("\n", " ")}'"))}]";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to summarize GitHub data.");
                return "Could not analyze repository data.";
            }
        }

        private async Task<string> GetPersonaFromAiAsync(string summary)
        {
            var systemPrompt = @"You are a 'Developer Persona Analyzer'... Your response must be a clean JSON object... with this exact structure:
{
  ""persona"": ""Persona Name in Georgian"",
  ""strengths"": ""A short, witty sentence about strengths in Georgian"",
  ""weaknesses"": ""A short, witty sentence about weaknesses in Georgian"",
  ""celebrity_developer"": ""A funny comparison... in Georgian""
}
    Do not add any extra fields. Do not wrap the JSON in markdown code blocks.";

            var userPrompt = $"Analyze the following repository summary and generate a developer persona: {summary}";

            if (_openAiClient == null)
            {
                _logger.LogWarning("OpenAI client not configured. Returning fallback persona.");
                return "{\"persona\": \"იდუმალი უცნობი\", \"strengths\": \"თქვენი შესაძლებლობები იმდენად დიდია, AI-საც კი უჭირს მათი გაანალიზება.\", \"weaknesses\": \"ზოგჯერ ზედმეტად იდუმალი ხართ.\", \"celebrity_developer\": \"სატოში ნაკამოტოსავით, თქვენი ნამდვილი ვინაობა უცნობია.\"}";
            }

            try
            {
                var modelName = _configuration["OpenAiSettings:RoastModel"] ?? "gpt-4o-mini";
                var chatClient = _openAiClient.GetChatClient(modelName);

                ChatCompletion completion = await chatClient.CompleteChatAsync(
                    new List<ChatMessage> { new SystemChatMessage(systemPrompt), new UserChatMessage(userPrompt) });

                string rawResponse = completion.Content[0].Text;
                _logger.LogInformation("OpenAI raw persona response: {RawResponse}", rawResponse);

                // ვასუფთავებთ პასუხს, რათა მხოლოდ სუფთა JSON დავტოვოთ
                int firstBrace = rawResponse.IndexOf('{');
                int lastBrace = rawResponse.LastIndexOf('}');
                if (firstBrace != -1 && lastBrace > firstBrace)
                {
                    string cleanJson = rawResponse.Substring(firstBrace, lastBrace - firstBrace + 1);
                    return cleanJson;
                }

                throw new InvalidOperationException("AI did not return a valid JSON object.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get persona from OpenAI. Returning fallback persona.");
                return "{\"persona\": \"AI-სთან კავშირი 끊断됨\", \"strengths\": \"თქვენი კოდი იმდენად ძლიერია, მან ხელოვნური ინტელექტი დააბნია.\", \"weaknesses\": \"შეიძლება დროა, ყავა დალიოთ.\", \"celebrity_developer\": \"სკაინეტთან, მის გააქტიურებამდე.\"}";
            }
        }
        private class GitHubAnalysisResponse { [JsonPropertyName("data")] public GqlData Data { get; set; } }
        private class GqlData { public GqlRepository Repository { get; set; } }
        private class GqlRepository { public GqlDefaultBranchRef DefaultBranchRef { get; set; } [JsonPropertyName("object")] public GqlObject Object { get; set; } }
        private class GqlDefaultBranchRef { public GqlTarget Target { get; set; } }
        private class GqlTarget { public GqlHistory History { get; set; } }
        private class GqlHistory { public List<GqlNode> Nodes { get; set; } }
        private class GqlNode { public string Message { get; set; } }
        private class GqlObject { public List<GqlEntry> Entries { get; set; } }
        private class GqlEntry { public string Name { get; set; } public string Type { get; set; } }
    }
}