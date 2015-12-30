using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Definition
{
    public interface ICodeBuilder
    {
        string Build(string template);
    }
}
