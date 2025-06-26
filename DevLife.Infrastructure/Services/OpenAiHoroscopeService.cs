using DevLife.Application.Interfaces;
using DevLife.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Services;

public class OpenAiHoroscopeService : IHoroscopeService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<OpenAiHoroscopeService> _logger;
    private readonly OpenAIClient _openAiClient;

    public OpenAiHoroscopeService(IConfiguration configuration, ILogger<OpenAiHoroscopeService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        var apiKey = _configuration["ApiKeys:OpenAiApiKey"];
        if (!string.IsNullOrEmpty(apiKey))
        {
            _openAiClient = new OpenAIClient(apiKey);
        }
    }

    public async Task<string> GetDailyHoroscopeAsync(ZodiacSign sign, CancellationToken cancellationToken = default)
    {
        if (_openAiClient == null)
        {
            return "AI ჰოროსკოპის სერვისი არ არის კონფიგურირებული.";
        }

        try
        {
            var messages = new List<ChatMessage>
                {
                    new SystemChatMessage("You are an eccentric and funny astrologer for software developers. Your horoscope predictions are witty, short (1-2 sentences), and relate to programming, bugs, or office life.ტექსტი დაწერე ქართულად"),
                    new UserChatMessage($"Write a horoscope for a {sign} developer for today.")
                };

            ChatClient chatClient = _openAiClient.GetChatClient("gpt-4o");

            var completionOptions = new ChatCompletionOptions
            {
                Temperature = 0.8f
            };

            ChatCompletion completion = await chatClient.CompleteChatAsync(messages, completionOptions, cancellationToken);

            return completion.Content[0].Text ?? "AI-მ დღეს დასვენება გადაწყვიტა.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while calling OpenAI API.");
            return "ხელოვნურ ინტელექტთან კავშირისას შეფერხებაა.";
        }
    }
}