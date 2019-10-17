using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDB.Client.Simple
{
    public static class MongoDbExtensions
    {
        public static void AddMongoDBContext(this IServiceCollection services, Action<MongoDbOptions> options = null)
        {
            var opt = new MongoDbOptions();
            options?.Invoke(opt);

            var settings = MongoClientSettings.FromConnectionString(opt.ConnectionString);
            settings.MaxConnectionIdleTime = TimeSpan.FromMinutes(opt.MaxIdleTime);
            settings.MaxConnectionLifeTime = TimeSpan.FromMinutes(opt.MaxLifeTime);
            settings.RetryWrites = true;

            var client = new MongoClient(settings);
            services.TryAddSingleton(client);

            services.TryAddSingleton(opt);
            services.TryAddSingleton<MongoDBContext>();
        }
    }
}
