using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

#nullable enable

namespace ApiMongo.Entities
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("User")]
        public string User { get; set; } = null!;

        [BsonElement("ActivityId")]
        public int ActivityId { get; set; }

        [BsonElement("PostTime")]
        public DateTime PostTime { get; set; }

        [BsonElement("Body")]
        public string Body { get; set; } = null!;
    }
}