using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    public interface IQueryParameterAppender
    {
        IQueryDefinitionBuilder Add(string name, object value);

        IQueryDefinitionBuilder Add(ParameterDefinition definition);

        IQueryDefinitionBuilder AddIf(bool condition, string name, object value);

        IQueryDefinitionBuilder AddIf(bool condition, ParameterDefinition definition);

    }
}
