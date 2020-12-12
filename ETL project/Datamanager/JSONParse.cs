using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Datamanager
{
    public class JSONParse : IParser
    {
        /* the only method of parser's interface we must implement */
        public T Parse<T>(string jsonConfigFileName) where T : new()
        {
            T options = new T();

            try
            {
                string jsonInner = File.ReadAllText(jsonConfigFileName);
                JsonDocument jsonDoc = JsonDocument.Parse(jsonInner);
                var element = jsonDoc.RootElement;

                if (typeof(ParseOptions).GetProperties().Any(prop => prop.Name == typeof(T).Name))
                {
                    element = element.GetProperty("ParseOptions");
                }
                element = element.EnumerateObject().Single(parameter => parameter.Name == typeof(T).Name).Value;
                options = JsonSerializer.Deserialize<T>(element.GetRawText());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return options;
        }
    }
}
