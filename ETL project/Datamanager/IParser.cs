using System;
using System.Text;
using System.Collections.Generic;

namespace Datamanager
{
    interface IParser
    {
        T Parse<T>(string configFileName) where T : new();
    }
}
