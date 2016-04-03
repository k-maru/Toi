using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    public interface IQueryTextAppender
    {
        IQueryDefinitionBuilder Add(string text);

        IQueryDefinitionBuilder AddIf(bool condition, string text);

        IQueryDefinitionBuilder RemovePrev();

        IQueryDefinitionBuilder RemovePrevIf(bool condition);

        IQueryDefinitionBuilder ReplacePrev(Func<string, string> replacer);

        string Prev();
    }
}
