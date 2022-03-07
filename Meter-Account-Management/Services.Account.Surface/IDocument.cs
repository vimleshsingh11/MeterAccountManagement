using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Services.Account.Surface
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }

        DateTime CreatedAt { get; }

        /// <summary>
        /// Unique identifier for account
        /// </summary>
        int AccountId { get; set; }
    }
}
