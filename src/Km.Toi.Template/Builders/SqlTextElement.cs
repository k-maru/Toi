using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    class SqlTextElement : ISqlDefinitionTextElement
    {
        private string text;

        public SqlTextElement(string text)
        {
            this.text = text;
        }

        SqlDefinitionElementType ISqlDefinitionElement.ElementType { get; } = SqlDefinitionElementType.Text;

        public string GetText()
        {
            return this.text;
        }
    }
}
