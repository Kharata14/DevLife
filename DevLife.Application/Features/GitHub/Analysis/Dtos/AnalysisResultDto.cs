using System;
using System.Text.Json;

namespace DevLife.Application.Features.GitHub.Analysis.Dtos
{
    public class AnalysisResultDto
    {
        public Guid JobId { get; set; }
        public string Status { get; set; }
        public JsonElement? Result { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}