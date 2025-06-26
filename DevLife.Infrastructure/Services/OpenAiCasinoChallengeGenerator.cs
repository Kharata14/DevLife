using DevLife.Application.Features.CodeCasino.Dtos;
using DevLife.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Services;
 public class OpenAiCasinoChallengeGenerator : ICasinoChallengeGenerator
{
    private readonly OpenAIClient _openAiClient;
    private readonly ILogger<OpenAiCasinoChallengeGenerator> _logger;

    public OpenAiCasinoChallengeGenerator(IConfiguration configuration, ILogger<OpenAiCasinoChallengeGenerator> logger)
    {
        var apiKey = configuration["ApiKeys:OpenAiApiKey"];
        _openAiClient = !string.IsNullOrEmpty(apiKey) ? new OpenAIClient(apiKey) : null;
        _logger = logger;
    }

    public async Task<CasinoChallengeDto> GenerateChallengeAsync(string language, string experienceLevel)
    {
        if (_openAiClient == null) throw new InvalidOperationException("OpenAI client is not configured.");

        var systemPrompt = @"You are an API that generates programming challenges. Your task is to create a subtle code challenge for a user... You must return a raw JSON object (nothing else) with the following structure:
            {
              ""description"": ""A short question about the code in Georgian"",
              ""snippets"": [
                { ""id"": ""correct_1"", ""code"": ""...correct code..."", ""isCorrect"": true },
                { ""id"": ""buggy_1"", ""code"": ""...buggy code..."", ""isCorrect"": false, ""explanation"": ""A brief explanation of the bug in Georgian"" }
              ]
            }
            The bug should be relevant to the user's experience level. For a 'Junior', a simple syntax or logical error. For a 'Middle', a misuse of a feature. For a 'Senior', a subtle performance or design pattern issue.";

        var userPrompt = $"Generate a challenge for a {experienceLevel} developer using {language}.";

        try
        {
            var chatClient = _openAiClient.GetChatClient("gpt-3.5-turbo");
            ChatCompletion completion = await chatClient.CompleteChatAsync(new List<ChatMessage> { new SystemChatMessage(systemPrompt), new UserChatMessage(userPrompt) });

            var jsonResponse = completion.Content[0].Text;
            _logger.LogInformation("OpenAI raw response for casino: {JsonResponse}", jsonResponse);

            return JsonSerializer.Deserialize<CasinoChallengeDto>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate casino challenge from OpenAI.");
            throw;
        }
    }
}
