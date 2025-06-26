using DevLife.Application.Features.CodeRoast.Dtos;
using DevLife.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Services;
public class Judge0CodeExecutionService : ICodeExecutionService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<Judge0CodeExecutionService> _logger;
    private const string Judge0ApiUrl = "https://judge0-ce.p.rapidapi.com/submissions?base64_encoded=false&wait=true";

    public Judge0CodeExecutionService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<Judge0CodeExecutionService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<CodeExecutionResultDto> ExecuteCodeAsync(string sourceCode, string language)
    {
        var apiKey = _configuration["ApiKeys:Judge0ApiKey"];
        var apiHost = _configuration["ApiKeys:Judge0ApiHost"];

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiHost))
        {
            _logger.LogError("Judge0 API Key or Host is not configured.");
            return new CodeExecutionResultDto { IsSuccess = false, Error = "Execution service not configured." };
        }

        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new System.Uri(Judge0ApiUrl),
            Headers =
                {
                    { "X-RapidAPI-Key", apiKey },
                    { "X-RapidAPI-Host", apiHost },
                },
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                source_code = sourceCode,
                language_id = MapLanguageToJudge0Id(language)
            }), Encoding.UTF8, "application/json")
        };

        try
        {
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<Judge0Response>();

            return new CodeExecutionResultDto
            {
                IsSuccess = result.status?.id == 3, // ID 3 ნიშნავს "Accepted" (წარმატებით შესრულდა)
                Output = result.stdout,
                Error = result.stderr ?? result.compile_output,
                ExecutionTime = result.time
            };
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error executing code via Judge0 API.");
            return new CodeExecutionResultDto { IsSuccess = false, Error = "Failed to execute code." };
        }
    }

    private int MapLanguageToJudge0Id(string language) => language.ToLower() switch
    {
        "csharp" => 51,
        "javascript" => 63,
        "python" => 71,
        "java" => 62,
        _ => 63 // Default to JavaScript
    };

    private class Judge0Response { public string stdout { get; set; } public string stderr { get; set; } public string compile_output { get; set; } public string time { get; set; } public Judge0Status status { get; set; } }
    private class Judge0Status { public int id { get; set; } }
}
