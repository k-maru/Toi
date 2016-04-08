using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    public interface ISqlTextAppender
    {
        ISqlDefinitionBuilder Add(string text);

        ISqlDefinitionBuilder AddIf(bool condition, string text);

        ISqlDefinitionBuilder RemovePrev();

        ISqlDefinitionBuilder RemovePrevIf(bool condition);

        ISqlDefinitionBuilder ReplacePrev(Func<string, string> replacer);

        string Prev();
    }
}
