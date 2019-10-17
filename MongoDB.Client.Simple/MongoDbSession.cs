using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Client.Simple
{
    public class MongoDbSession : IDisposable
    {
        MongoDBContext Context;
        public IClientSessionHandle Session;
        public IMongoDatabase Database;

        private Dictionary<Type, object> Collections { get; set; } = new Dictionary<Type, object>();
        public MongoDbSession(MongoDBContext Context)
        {
            this.Context = Context;
            Session = Context.Client.StartSession();
            Database = Session.Client.GetDatabase(Context.Options.DatabaseName);
            Session.StartTransaction();
            Ping();
        }

        public void Ping()
        {
            try
            {
                Database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait();
            }
            catch (Exception err)
            {
                Console.WriteLine("Failed to ping MongoDB -> " + err.ToString());
            }
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

        public void Abort()
        {
            Session.AbortTransaction();
        }

        public void Commit()
        {
            Session.CommitTransaction();
        }

        public async Task AbortAsync()
        {
            await Session.AbortTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await Session.CommitTransactionAsync();
        }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}
