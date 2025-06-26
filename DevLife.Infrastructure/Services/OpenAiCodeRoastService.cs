using DevLife.Application.Features.CodeRoast.Dtos;
using DevLife.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Services;
public class OpenAiCodeRoastService : IAiCodeRoastService
{
    private readonly OpenAIClient _openAiClient;
    private readonly ILogger<OpenAiCodeRoastService> _logger;

    public OpenAiCodeRoastService(IConfiguration configuration, ILogger<OpenAiCodeRoastService> logger)
    {
        var apiKey = configuration["ApiKeys:OpenAiApiKey"];
        _openAiClient = !string.IsNullOrEmpty(apiKey) ? new OpenAIClient(apiKey) : null;
        _logger = logger;
    }

    public async Task<string> GetRoastAsync(string challengeDescription, string userCode, CodeExecutionResultDto executionResult)
    {
        if (_openAiClient == null) return "AI Roaster is not configured.";

        var systemPrompt = "You are a witty, sarcastic, but ultimately helpful senior developer reviewing a junior's code. Provide your feedback in Georgian.";
        var userPrompt = $@"
                The original task was: '{challengeDescription}'.
                The user submitted this code: 
                ```
                {userCode}
                ```
                The code execution result was: '{(executionResult.IsSuccess ? "Success! Output: " + executionResult.Output : "Failure. Error: " + executionResult.Error)}'.

                Your task is to provide a ""roast"".
                - If the code is good and works, give praise but in a funny, slightly condescending way. (e.g., ""ბრავო! ამ კოდს ჩემი ბებიაც დაწერდა, მაგრამ მაინც კარგია."")
                - If the code is bad or has errors, be funny and merciless. (e.g., ""ეს კოდი ისე ცუდია, კომპილატორმა დეპრესია დაიწყო."")
                - Keep the roast to 1-2 witty sentences.";

        try
        {
            var chatClient = _openAiClient.GetChatClient("gpt-4o-mini");

            ChatCompletion completion = await chatClient.CompleteChatAsync(
                new List<ChatMessage> { new SystemChatMessage(systemPrompt), new UserChatMessage(userPrompt) });

            return completion.Content[0].Text;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get roast from OpenAI.");
            return "AI-ს დღეს იუმორის გრძნობა გაუფუჭდა. ცადეთ მოგვიანებით.";
        }
    }
}