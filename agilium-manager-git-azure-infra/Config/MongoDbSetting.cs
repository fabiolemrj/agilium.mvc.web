using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Config
{
    public class MongoDbSetting
    {
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }

    }
}
