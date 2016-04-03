using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    class QueryParameterElement : IQueryDefinitionParameterElement, IParameterHolder
    {
        private ParameterDefinition definition;

        public QueryParameterElement(string name, object value)
        {
            this.definition = new ParameterDefinition(name, value);
        }

        public QueryParameterElement(ParameterDefinition definition)
        {
            this.definition = definition;
        }


        QueryDefinitionElementType IQueryDefinitionElement.ElementType { get; } = QueryDefinitionElementType.Parameter;

        public IEnumerable<ParameterDefinition> GetParameters()
        {
            yield return definition;
        }
    }
}
