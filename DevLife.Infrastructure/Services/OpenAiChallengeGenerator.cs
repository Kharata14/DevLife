using DevLife.Application.Features.CodeRoast.Dtos;
using DevLife.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Services
{
    public class OpenAiChallengeGenerator : ICodingChallengeProvider
    {
        private readonly OpenAIClient _openAiClient;
        private readonly ILogger<OpenAiChallengeGenerator> _logger;

        public OpenAiChallengeGenerator(IConfiguration configuration, ILogger<OpenAiChallengeGenerator> logger)
        {
            var apiKey = configuration["ApiKeys:OpenAiApiKey"];
            _openAiClient = !string.IsNullOrEmpty(apiKey) ? new OpenAIClient(apiKey) : null;
            _logger = logger;
        }

        public async Task<CodingChallengeDto> GetChallengeAsync(string language, string difficulty)
        {
            if (_openAiClient == null) throw new InvalidOperationException("OpenAI client is not configured.");
            var systemPrompt = @"You are an API that generates programming challenges. 
                    Your task is to create a coding challenge for a user with a specific language and experience level.
                    You must return only a raw JSON object (no other text or explanations) with the following structure:
                   {
                    ""id"": ""a_unique_challenge_id"",
                    ""title"": ""A short challenge title in Georgian"",
                    ""description"": ""A clear challenge description in Georgian"",
                    ""language"": ""{language}"",
                    ""difficulty"": ""{difficulty}"",
                    ""boilerplateCode"": [ ""line 1 of code"", ""line 2 of code"", ""line 3 of code"" ]
                   }
                    The 'boilerplateCode' must be a JSON array of strings.
                    The challenge complexity must match the experience level...";

            var userPrompt = $"Generate a challenge for a '{difficulty}' developer using '{language}'.";
            try
            {
                var chatClient = _openAiClient.GetChatClient("gpt-4o-mini");
                ChatCompletion completion = await chatClient.CompleteChatAsync(
                    new List<ChatMessage> { new SystemChatMessage(systemPrompt), new UserChatMessage(userPrompt) });

                var jsonResponse = completion.Content[0].Text;
                _logger.LogInformation("OpenAI challenge response: {JsonResponse}", jsonResponse);
                return JsonSerializer.Deserialize<CodingChallengeDto>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate challenge from OpenAI.");
                throw;
            }
        }
    }
}