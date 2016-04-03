using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    public interface IQueryDefinitionBuilder
    {

        ITemplateOptions Options { get; }

        IQueryTextAppender Text { get; }

        IQueryParameterAppender Parameter { get; }

        IQueryDefinitionBuilder StartBlock(string name);

        IQueryDefinitionBuilder EndBlock();

        IQueryDefinitionBuilder UseBlock(string name);
    }
}
