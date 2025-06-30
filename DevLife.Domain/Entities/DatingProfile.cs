// In: Domain/Entities/DatingProfile.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DevLife.Domain.Entities
{
    public class DatingProfile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("bio")]
        public string Bio { get; set; }

        [BsonElement("profileImageUrl")]
        public string ProfileImageUrl { get; set; }

        [BsonElement("techStack")]
        public List<string> TechStack { get; set; }

        [BsonElement("aiPersonality")]
        public string AiPersonality { get; set; }
    }
}