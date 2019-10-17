using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDB.Client.Simple
{
    public class MongoDbOptions
    {
        public string ConnectionString;
        public string DatabaseName;
        public int MaxIdleTime = 1;
        public int MaxLifeTime = 1;
    }
}
