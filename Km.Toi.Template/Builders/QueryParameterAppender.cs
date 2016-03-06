using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    class QueryParameterAppender : IQueryParameterAppender
    {
        private QueryDefinitionBuilder builder = null;
        private CompositeQueryDefinitionElement elements = null;

        public QueryParameterAppender(QueryDefinitionBuilder builder, CompositeQueryDefinitionElement elements)
        {
            this.builder = builder;
            this.elements = elements;
        }

        public IQueryDefinitionBuilder Add(ParameterDefinition definition)
        {
            this.elements.Add(new QueryParameterElement(definition));
            return this.builder;
        }

        public IQueryDefinitionBuilder Add(string name, object value)
        {
            this.Add(new ParameterDefinition(name, value));
            return this.builder;
        }

        public IQueryDefinitionBuilder AddIf(bool condition, ParameterDefinition definition)
        {
            if (condition)
            {
                this.Add(definition);
            }
            return this.builder;
        }

        public IQueryDefinitionBuilder AddIf(bool condition, string name, object value)
        {
            if (condition)
            {
                this.Add(name, value);
            }
            return this.builder;
        }
    }
}
