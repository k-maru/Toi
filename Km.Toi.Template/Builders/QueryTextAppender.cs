using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    class QueryTextAppender : IQueryTextAppender
    {
        private QueryDefinitionBuilder builder = null;
        private CompositeQueryDefinitionElement elements = null;

        public QueryTextAppender(QueryDefinitionBuilder builder, CompositeQueryDefinitionElement elements)
        {
            this.builder = builder;
            this.elements = elements;
        }

        public IQueryDefinitionBuilder Add(string text)
        {
            this.elements.Add(new QueryTextElement(text));
            return this.builder;
        }

        public IQueryDefinitionBuilder AddIf(bool condition, string text)
        {
            if (condition)
            {
                this.Add(text);
            }
            return this.builder;
        }

        public string Prev()
        {
            var prev = this.elements.Prev(QueryDefinitionElementType.Text) as IQueryDefinitionTextElement;
            if (prev != null) {
                return prev.GetText();
            }
            return null;
        }

        public IQueryDefinitionBuilder RemovePrev()
        {
            this.elements.RemovePrev(QueryDefinitionElementType.Text);
            return this.builder;
        }

        public IQueryDefinitionBuilder RemovePrevIf(bool condition)
        {
            if (condition)
            {
                this.RemovePrev();
            }
            return this.builder;
        }

        public IQueryDefinitionBuilder ReplacePrev(Func<string, string> replacer)
        {
            var prev = this.elements.Prev(QueryDefinitionElementType.Text) as IQueryDefinitionTextElement;
            if(prev != null)
            {
                var newText = replacer(prev.GetText());
                this.elements.ReplacePrev(new QueryTextElement(newText)); 
            }
            return this.builder;
        }
    }
}
