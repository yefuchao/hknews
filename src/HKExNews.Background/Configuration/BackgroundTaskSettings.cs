using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HKExNews.Background.Configuration
{
    public class BackgroundTaskSettings
    {
        public string ConnectionString { get; set; }

        public int CheckUpdateTime { get; set; }

        /// <summary>
        /// 1 sqlserver ,2 mysql
        /// </summary>
        public int DateBaseType { get; set; }
    }
}
