using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    public sealed class SqlDefinitionBuilder : ISqlDefinitionBuilder
    {
        private CompositeSqlDefinitionElement root;

        public SqlDefinitionBuilder(ITemplateOptions options = null)
        {
            this.root = new CompositeSqlDefinitionElement(this);
            this.Text = new SqlTextAppender(this, root);
            this.Parameter = new SqlParameterAppender(this, root);
            this.Options = options;
        }

        public ITemplateOptions Options { get; }

        public ISqlTextAppender Text { get; }

        public ISqlParameterAppender Parameter { get; }

        public ISqlDefinitionBuilder EndBlock()
        {
            root.EndBlock();
            return this;
        }

        public ISqlDefinitionBuilder StartBlock(string name)
        {
            root.StartBlock(name);
            return this;
        }

        public ISqlDefinitionBuilder UseBlock(string name)
        {
            root.UseBlock(name);
            return this;
        }

        public SqlDefinition Build()
        {
            var sqlText = root.GetText().Trim();
            var result = new SqlDefinition(sqlText);
            result.Parameters.AddRange(root.GetParameters());
            return result;
        }
    }
}
