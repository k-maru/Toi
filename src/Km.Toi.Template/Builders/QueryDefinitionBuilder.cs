using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    public sealed class QueryDefinitionBuilder : IQueryDefinitionBuilder
    {
        private CompositeQueryDefinitionElement root;

        public QueryDefinitionBuilder(ITemplateOptions options = null)
        {
            this.root = new CompositeQueryDefinitionElement(this);
            this.Text = new QueryTextAppender(this, root);
            this.Parameter = new QueryParameterAppender(this, root);
            this.Options = options;
        }

        public ITemplateOptions Options { get; }

        public IQueryTextAppender Text { get; }

        public IQueryParameterAppender Parameter { get; }

        public IQueryDefinitionBuilder EndBlock()
        {
            root.EndBlock();
            return this;
        }

        public IQueryDefinitionBuilder StartBlock(string name)
        {
            root.StartBlock(name);
            return this;
        }

        public IQueryDefinitionBuilder UseBlock(string name)
        {
            root.UseBlock(name);
            return this;
        }

        public QueryDefinition Build()
        {
            var queryText = root.GetText().Trim();
            var result = new QueryDefinition(queryText);
            result.Parameters.AddRange(root.GetParameters());
            return result;
        }
    }
}
