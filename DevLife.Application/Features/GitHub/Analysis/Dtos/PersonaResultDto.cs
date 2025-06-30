using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DevLife.Application.Features.GitHub.Analysis.Dtos
{
    public class PersonaResultDto
    {
        [JsonPropertyName("persona")]
        public string Persona { get; set; }

        [JsonPropertyName("strengths")]
        public string Strengths { get; set; }

        [JsonPropertyName("weaknesses")]
        public string Weaknesses { get; set; }

        [JsonPropertyName("celebrity_developer")]
        public string CelebrityDeveloper { get; set; }
    }
}