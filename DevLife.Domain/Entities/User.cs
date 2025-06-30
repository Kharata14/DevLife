using DevLife.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string TechStack { get; set; }
    public string ExperienceLevel { get; set; }
    public ZodiacSign ZodiacSign { get; set; }
    public int Points { get; set; }
    public int CurrentStreak { get; set; } = 0;
    public int LongestStreak { get; set; } = 0;
    public string? GitHubAccessToken { get; set; }
    public string? Bio { get; set; }
    public Gender? Gender { get; set; }
    public Interest? InterestedIn { get; set; }
}

