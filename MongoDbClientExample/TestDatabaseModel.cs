using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDbClientExample
{
    public class TestDatabaseModel
    {
        public Guid ModelID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
