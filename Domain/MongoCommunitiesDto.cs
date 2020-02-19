using Community.Api.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commuinity.Api.Domain
{
    public class MongoCommunitiesDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoPath { get; set; }
        public List<Company> Companies { get; set; }
        public List<Developer> Developers { get; set; }
        public List<Organization> Organizations { get; set; }
        public List<Post> Posts { get; set; }


    }
}
