using DevLife.Application.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Repositories;
public class MongoExcuseTemplateRepository : IExcuseTemplateRepository
{
    private readonly IMongoCollection<ExcuseTemplate> _templates;

    public MongoExcuseTemplateRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("devlife");
        _templates = database.GetCollection<ExcuseTemplate>("excuseTemplates");
    }

    public async Task<string> GetRandomTemplateAsync(string category, CancellationToken cancellationToken = default)
    {
        var templates = await _templates.AsQueryable()
                                        .Where(t => t.Category == category)
                                        .Sample(1)
                                        .FirstOrDefaultAsync(cancellationToken);

        return templates?.Template ?? "რაღაც გაუთვალისწინებელი მოხდა.";
    }
}

// დამხმარე კლასი MongoDB დოკუმენტისთვის
public class ExcuseTemplate
{
    public MongoDB.Bson.ObjectId Id { get; set; }
    public string Category { get; set; }
    public string Template { get; set; }
}