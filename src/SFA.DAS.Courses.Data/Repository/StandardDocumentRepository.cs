using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Polly;
using Polly.Retry;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class StandardDocumentRepository : IStandardDocumentRepository
    {
        private readonly RetryPolicy retryPolicy;
        private readonly IMongoCollection<BsonDocument> standardCollection;
        public StandardDocumentRepository()
        {
            var client = new MongoClient("mongodb://localhost:C2y6yDjf5%2FR%2Bob0N8A7Cgv30VRDJIWEHLM%2B4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw%2FJw%3D%3D@localhost:10255/admin?ssl=true");
            var database = client.GetDatabase("standards");
            standardCollection = database.GetCollection<BsonDocument>("standards");
            retryPolicy = Policy.Handle<MongoException>().WaitAndRetry(new[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(4) });
        }

        public string SaveDocument(string key, string content)
        {
            var document = BsonDocument.Parse(content);
            document.Add(new BsonElement("_id", key));
            //await retryPolicy.ExecuteAsync(_ => standardCollection.ReplaceOneAsync(new BsonDocument("_id", key), document), new Context(nameof(SaveDocument)));
            retryPolicy.Execute(_ => standardCollection.InsertOne(document), new Context(nameof(SaveDocument)));
            return key;
        }
    }
}
