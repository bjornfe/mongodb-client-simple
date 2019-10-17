using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace MongoDB.Client.Simple
{
    public class MongoDBContext
    {

        public MongoClient Client;
        public MongoDbOptions Options;
        public IMongoDatabase Database;

        public event Action<string> DebugText;

        private Dictionary<Type, object> Collections { get; set; } = new Dictionary<Type, object>();

        public MongoDBContext(MongoClient client, MongoDbOptions options)
        {
            this.Client = client;
            this.Options = options;
            Database = Client.GetDatabase(options.DatabaseName);
        }

        public void Ping()
        {
            try
            {
                Database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait();
            }
            catch (Exception err)
            {
                DebugText?.Invoke("Failed to ping MongoDB -> " + err.ToString());
            }
        }

        public MongoDbSession CreateSession()
        {
            return new MongoDbSession(this);
        }

        public IMongoCollection<T> GetCollection<T>(string Name = null)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap<T>(c =>
                {
                    c.AutoMap();
                    c.SetIgnoreExtraElements(true);
                });
            }

            if (Collections.ContainsKey(typeof(T)))
            {
                var obj = Collections[typeof(T)];
                if (obj is IMongoCollection<T> col)
                    return col;
            }

            string name = typeof(T).Name;
            if (Name != null)
                name = Name;


            var collection = Database.GetCollection<T>(name);
            Collections[typeof(T)] = collection;

            return collection;

        }
    }
}
