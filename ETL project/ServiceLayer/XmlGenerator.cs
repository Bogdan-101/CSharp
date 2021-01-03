using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class XmlGenerator<T> : IXmlGenerator
    {
        private readonly string pathToXml;

        public XmlGenerator(string path)
        {
            pathToXml = path;
        }

        public async Task XmlGenerate<T>(IEnumerable<T> info)
        {
            await Task.Run(() =>
            {
                try
                {
                    List<T> emp = new List<T>(info);

                    XmlSerializer formatter = new XmlSerializer(typeof(List<T>));

                    using (FileStream fs = new FileStream(pathToXml, FileMode.OpenOrCreate))
                    {
                        formatter.Serialize(fs, emp);
                    }
                }
                catch (Exception trouble)
                {
                    throw trouble;
                }
            });
        }
    }
}
