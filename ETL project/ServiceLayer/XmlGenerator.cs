using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ServiceLayer
{
    public class XmlGenerator<T> : IXmlGenerator
    {
        private readonly string pathToXml;

        public XmlGenerator(string path)
        {
            pathToXml = path;
        }

        public void XmlGenerate<T>(IEnumerable<T> info)
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

        }
    }
}
