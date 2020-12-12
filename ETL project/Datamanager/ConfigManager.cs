using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Datamanager
{
    class ConfigManager
    {
        private readonly IParser configParser;

        public ConfigManager(IParser configParser)
        {
            this.configParser = configParser;
        }

        public T GetOptions<T>(string fileOptionsPath) where T : new()
        {
            return configParser.Parse<T>(fileOptionsPath);
        }
    }
}
