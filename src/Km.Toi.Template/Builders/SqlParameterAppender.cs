using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    class SqlParameterAppender : ISqlParameterAppender
    {
        private SqlDefinitionBuilder builder = null;
        private CompositeSqlDefinitionElement elements = null;

        public SqlParameterAppender(SqlDefinitionBuilder builder, CompositeSqlDefinitionElement elements)
        {
            this.builder = builder;
            this.elements = elements;
        }

        public ISqlDefinitionBuilder Add(ParameterDefinition definition)
        {
            this.elements.Add(new SqlParameterElement(definition));
            return this.builder;
        }

        public ISqlDefinitionBuilder Add(string name, object value)
        {
            this.Add(new ParameterDefinition(name, value));
            return this.builder;
        }

        public ISqlDefinitionBuilder AddIf(bool condition, ParameterDefinition definition)
        {
            if (condition)
            {
                this.Add(definition);
            }
            return this.builder;
        }

        public ISqlDefinitionBuilder AddIf(bool condition, string name, object value)
        {
            if (condition)
            {
                this.Add(name, value);
            }
            return this.builder;
        }
    }
}
