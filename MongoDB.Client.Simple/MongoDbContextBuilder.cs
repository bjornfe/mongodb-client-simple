using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDB.Client.Simple
{
    public class MongoDbContextBuilder
    {
        MongoDbOptions opt = new MongoDbOptions();
        public MongoDbContextBuilder()
        {

        }

        public MongoDbContextBuilder SetOptions(Action<MongoDbOptions> options)
        {
            options?.Invoke(opt);
            return this;
        }

        public MongoDBContext Build()
        {
            var settings = MongoClientSettings.FromConnectionString(opt.ConnectionString);
            settings.MaxConnectionIdleTime = TimeSpan.FromMinutes(opt.MaxIdleTime);
            settings.MaxConnectionLifeTime = TimeSpan.FromMinutes(opt.MaxLifeTime);
            settings.RetryWrites = true;

            var client = new MongoClient(settings);

            return new MongoDBContext(client, opt);

        }


    }
}
