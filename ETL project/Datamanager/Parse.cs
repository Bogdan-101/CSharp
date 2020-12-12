using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net;

namespace Datamanager
{
    public class Parse
    {
        public ParseOptions options = new ParseOptions();
        public Parse()
        {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string baseDirectoryName = basePath.Substring(0, basePath.Length - 14);
        string xmlConfigurationFileName = baseDirectoryName + "xmlConfig.xml";
        string jsonConfigurationFileName = baseDirectoryName + "appsettings.json";
        if (File.Exists(xmlConfigurationFileName))
            {
                XMLParse parser = new XMLParse();
                ConfigManager configurationManager = new ConfigManager(parser);

                this.options.EncryptingOptions = configurationManager.GetOptions<EncryptingOptions>(xmlConfigurationFileName);
                this.options.PathsOptions = configurationManager.GetOptions<PathsOptions>(xmlConfigurationFileName);
                this.options.CompressOptions = configurationManager.GetOptions<CompressOptions>(xmlConfigurationFileName);
            }
            else if (File.Exists(jsonConfigurationFileName))
            {
                Console.WriteLine(jsonConfigurationFileName);
                JSONParse parser = new JSONParse();
                ConfigManager configurationManager = new ConfigManager(parser);

                this.options.EncryptingOptions = configurationManager.GetOptions<EncryptingOptions>(jsonConfigurationFileName);
                this.options.PathsOptions = configurationManager.GetOptions<PathsOptions>(jsonConfigurationFileName);
                this.options.CompressOptions = configurationManager.GetOptions<CompressOptions>(jsonConfigurationFileName);
            }
            else
            {
                throw new IOException("Configuration file with incorrect extension");
            }
        }
    }
}
