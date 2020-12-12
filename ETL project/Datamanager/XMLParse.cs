using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Datamanager
{
    class XMLParse : IParser
    {
        public T Parse<T>(string xmlConfigurationFileName) where T : new()
        {
            T options = new T();

            try
            {
                Validate(xmlConfigurationFileName);

                string xmlInner = "";
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                XDocument xDoc = XDocument.Load(xmlConfigurationFileName);
                var nodes = xDoc.Document.Descendants();

                foreach (var node in nodes)
                {
                    if (node.Name == typeof(T).Name)
                    {
                        xmlInner = node.ToString();
                        break;
                    }
                }

                using (TextReader tReader = new StringReader(xmlInner))
                {
                    options = (T)formatter.Deserialize(tReader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return options;
        }
        private void Validate(string xmlConfigFileName)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(null, Path.ChangeExtension(xmlConfigFileName, "xsd"));

            XDocument xDoc = XDocument.Load(xmlConfigFileName);

            xDoc.Validate(schemas, (sender, validationEventArgs) =>
            {
                if (validationEventArgs.Message.Length != 0)
                {
                    throw validationEventArgs.Exception;
                }
            });
        }
    }
}
