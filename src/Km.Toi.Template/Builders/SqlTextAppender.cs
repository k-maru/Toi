using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    class SqlTextAppender : ISqlTextAppender
    {
        private SqlDefinitionBuilder builder = null;
        private CompositeSqlDefinitionElement elements = null;

        public SqlTextAppender(SqlDefinitionBuilder builder, CompositeSqlDefinitionElement elements)
        {
            this.builder = builder;
            this.elements = elements;
        }

        public ISqlDefinitionBuilder Add(string text)
        {
            this.elements.Add(new SqlTextElement(text));
            return this.builder;
        }

        public ISqlDefinitionBuilder AddIf(bool condition, string text)
        {
            if (condition)
            {
                this.Add(text);
            }
            return this.builder;
        }

        public string Prev()
        {
            var prev = this.elements.Prev(SqlDefinitionElementType.Text) as ISqlDefinitionTextElement;
            if (prev != null) {
                return prev.GetText();
            }
            return null;
        }

        public ISqlDefinitionBuilder RemovePrev()
        {
            this.elements.RemovePrev(SqlDefinitionElementType.Text);
            return this.builder;
        }

        public ISqlDefinitionBuilder RemovePrevIf(bool condition)
        {
            if (condition)
            {
                this.RemovePrev();
            }
            return this.builder;
        }

        public ISqlDefinitionBuilder ReplacePrev(Func<string, string> replacer)
        {
            var prev = this.elements.Prev(SqlDefinitionElementType.Text) as ISqlDefinitionTextElement;
            if(prev != null)
            {
                var newText = replacer(prev.GetText());
                this.elements.ReplacePrev(new SqlTextElement(newText)); 
            }
            return this.builder;
        }
    }
}
