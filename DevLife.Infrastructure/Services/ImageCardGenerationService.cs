using DevLife.Application.Features.GitHub.Analysis.Dtos;
using DevLife.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Services
{
    public class ImageCardGenerationService : ICardGenerationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly ILogger<ImageCardGenerationService> _logger;

        public ImageCardGenerationService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<ImageCardGenerationService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            // ვკითხულობთ გასაღებს User Secrets-იდან და არა Environment Variable-იდან
            _apiKey = configuration["ApiKeys:OpenAiApiKey"];
        }

        public async Task<byte[]?> GenerateCardAsync(PersonaResultDto persona)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                _logger.LogError("OpenAI API Key is not configured in User Secrets.");
                return null;
            }

            _logger.LogInformation("Starting DALL-E 3 image generation for persona: {Persona}", persona.Persona);

            var prompt = $@"A funny, expressive digital art character of a software developer, representing the personality '{persona.Persona}'. The developer's main strength is '{string.Join(", ", persona.Strengths)}'. The style should be modern, quirky, and fun, with exaggerated features, maybe interacting with a laptop or a spilling coffee mug. Simple, vibrant background.";

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                var requestBody = new
                {
                    model = "dall-e-3",
                    prompt = prompt,
                    n = 1,
                    size = "1024x1024"
                };

                var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/images/generations", requestBody);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("AI Image Generation Failed with status {StatusCode}: {ErrorContent}", response.StatusCode, errorContent);
                    return null;
                }

                var imageResponse = await response.Content.ReadFromJsonAsync<OpenAIImageResponse>();

                if (imageResponse != null && imageResponse.Data.Any())
                {
                    var imageUrl = imageResponse.Data[0].Url;
                    _logger.LogInformation("Image URL received. Downloading image...");

                    var imageDownloaderClient = _httpClientFactory.CreateClient();
                    var imageBytes = await imageDownloaderClient.GetByteArrayAsync(imageUrl);

                    _logger.LogInformation("Image downloaded successfully ({SizeInKB} KB).", imageBytes.Length / 1024);
                    return imageBytes;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred during AI image generation.");
            }

            return null;
        }

        private class OpenAIImageResponse
        {
            [JsonPropertyName("data")]
            public List<ImageData> Data { get; set; } = new();
        }

        private class ImageData
        {
            [JsonPropertyName("url")]
            public string Url { get; set; } = default!;
        }
    }
}