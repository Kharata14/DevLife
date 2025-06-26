using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<ExcuseLog> ExcuseLogs { get; set; }
    public DbSet<CasinoGame> CasinoGames { get; set; }
    public DbSet<BugChaseGameScore> BugChaseGameScores { get; set; }
    public DbSet<UserAchievement> UserAchievements { get; set; }
}
