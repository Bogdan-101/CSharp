using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    interface IXmlGenerator
    {
        Task XmlGenerate<T>(IEnumerable<T> info);
    }
}
