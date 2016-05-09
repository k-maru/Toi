using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    public interface ISqlParameterAppender
    {
        ISqlDefinitionBuilder Add(string name, object value);

        ISqlDefinitionBuilder Add(ParameterDefinition definition);

        ISqlDefinitionBuilder AddIf(bool condition, string name, object value);

        ISqlDefinitionBuilder AddIf(bool condition, ParameterDefinition definition);

        IEnumerable<string> GetNames();
    }
}
