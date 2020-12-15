using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer
{
    interface IXmlGenerator
    {
        void XmlGenerate<T>(IEnumerable<T> info);
    }
}
