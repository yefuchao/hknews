using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Configuration
{
    public class APISettings
    {
        public string ConnectionString { get; set; }

        public string EventBusConnection { get; set; }
    }
}
