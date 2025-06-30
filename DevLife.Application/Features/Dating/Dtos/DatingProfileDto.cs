public class DatingProfileDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Bio { get; set; }
    public string ProfileImageUrl { get; set; }
    public List<string> TechStack { get; set; }
    public int CompatibilityScore { get; set; }
}