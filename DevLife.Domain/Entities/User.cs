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
    public int Points { get; set; } = 0;
}

