namespace DevLife.Application.Features.GitHub.Dtos
{
    public class GitHubRepoDto
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string PrimaryLanguage { get; set; }
    }
}