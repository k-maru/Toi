using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    class QueryTextElement : IQueryDefinitionTextElement
    {
        private string text;

        public QueryTextElement(string text)
        {
            this.text = text;
        }

        QueryDefinitionElementType IQueryDefinitionElement.ElementType { get; } = QueryDefinitionElementType.Text;

        public string GetText()
        {
            return this.text;
        }
    }
}
