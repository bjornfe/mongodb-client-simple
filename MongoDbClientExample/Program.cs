
using MongoDB.Client.Simple;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;

namespace MongoDbClientExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new MongoDbContextBuilder()
                .SetOptions(opt =>
                {
                    opt.ConnectionString = "<your connection string>";
                    opt.DatabaseName = "<your database>";
                    opt.MaxLifeTime = 1;
                    opt.MaxIdleTime = 1;
                })
                .Build();


            IMongoCollection<TestDatabaseModel> modelCollection = context.GetCollection<TestDatabaseModel>("testModels");

            var filter = Builders<TestDatabaseModel>.Filter.Empty;
            var models = modelCollection
                .Find(filter)
                .ToList();

            foreach(var m in models)
            {
                Console.WriteLine(JsonConvert.SerializeObject(m));
            }

            var newModel = new TestDatabaseModel()
            {
                Name = "TestModel"
            };

            modelCollection.InsertOne(newModel);


        }
    }
}
