using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using DevLife.Domain.Enums;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Repositories
{
    public class MongoDatingProfileRepository : IDatingProfileRepository
    {
        private readonly IMongoCollection<DatingProfile> _profiles;

        public MongoDatingProfileRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("devlife");
            _profiles = database.GetCollection<DatingProfile>("datingProfiles");
        }

        public async Task<List<DatingProfile>> GetPotentialMatchesAsync(User currentUser, List<string> alreadySwipedIds)
        {
            var filterBuilder = Builders<DatingProfile>.Filter;
            var filters = new List<FilterDefinition<DatingProfile>>
            {
                filterBuilder.Nin(p => p.Id, alreadySwipedIds)
            };

            if (currentUser.InterestedIn == Interest.Females)
            {
                filters.Add(filterBuilder.Eq(p => p.Gender, "Female"));
            }
            else if (currentUser.InterestedIn == Interest.Males)
            {
                filters.Add(filterBuilder.Eq(p => p.Gender, "Male"));
            }

            var finalFilter = filterBuilder.And(filters);
            var potentialMatches = await _profiles.Find(finalFilter).ToListAsync();

            var userTechStack = currentUser.TechStack?.Split(',').Select(t => t.Trim()) ?? Enumerable.Empty<string>();

            var sortedMatches = potentialMatches.Select(profile => new
            {
                Profile = profile,
                CompatibilityScore = profile.TechStack.Intersect(userTechStack).Count()
            })
            .OrderByDescending(x => x.CompatibilityScore)
            .Select(x => x.Profile)
            .ToList();

            return sortedMatches;
        }
    }
}