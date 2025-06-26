namespace DevLife.Application.Features.CodeRoast.Dtos
{
    public class CodingChallengeDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string Difficulty { get; set; }
        public List<string> BoilerplateCode { get; set; }
    }
}