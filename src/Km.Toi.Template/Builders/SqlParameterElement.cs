using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    class SqlParameterElement : ISqlDefinitionParameterElement, IParameterHolder
    {
        private ParameterDefinition definition;

        public SqlParameterElement(string name, object value)
        {
            this.definition = new ParameterDefinition(name, value);
        }

        public SqlParameterElement(ParameterDefinition definition)
        {
            this.definition = definition;
        }


        SqlDefinitionElementType ISqlDefinitionElement.ElementType { get; } = SqlDefinitionElementType.Parameter;

        public IEnumerable<ParameterDefinition> GetParameters()
        {
            yield return definition;
        }

        public IEnumerable<ParameterDefinition> GetAllParameters()
        {
            return GetParameters();
        }
    }
}
