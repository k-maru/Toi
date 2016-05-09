using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    public interface IParameterHolder
    {
        IEnumerable<ParameterDefinition> GetParameters();

        IEnumerable<ParameterDefinition> GetAllParameters();
    }
}
