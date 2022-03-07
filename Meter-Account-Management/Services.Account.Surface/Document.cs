using System;
using MongoDB.Bson;

namespace Services.Account.Surface
{
    public class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => DateTime.UtcNow;

        public int AccountId { get; set; }
    }
}
