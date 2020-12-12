using System;
using System.Collections.Generic;
using System.Text;

namespace Datamanager
{
    [Serializable]
    public class ParseOptions
    {
        public PathsOptions PathsOptions { get; set; }
        public EncryptingOptions EncryptingOptions { get; set; }
        public CompressOptions CompressOptions { get; set; }

        public ParseOptions()
        {
            this.PathsOptions = new PathsOptions();
            this.EncryptingOptions = new EncryptingOptions();
            this.CompressOptions = new CompressOptions();
        }
    }

    [Serializable]
    public class PathsOptions
    {
        public string SourceDirectory { get; set; }
        public string TargetPath { get; set; }
        public string Regex { get; set; }
    }

    [Serializable]
    public class EncryptingOptions
    {
        public string Key { get; set; }
    }

    [Serializable]
    public class CompressOptions
    {
        public string Extension { get; set; }
    }
}
