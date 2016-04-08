using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    public interface ISqlDefinitionBuilder
    {

        ITemplateOptions Options { get; }

        ISqlTextAppender Text { get; }

        ISqlParameterAppender Parameter { get; }

        ISqlDefinitionBuilder StartBlock(string name);

        ISqlDefinitionBuilder EndBlock();

        ISqlDefinitionBuilder UseBlock(string name);
    }
}
