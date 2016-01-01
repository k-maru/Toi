using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Definition
{
    public interface IQueryTextBuilder
    {
        IQueryTextBuilder Add(string value);

        IQueryTextBuilder AddIf(bool condition, string value);

        string GetPrev();

        IQueryTextBuilder RemovePrev();

        IQueryTextBuilder RemovePrevIf(bool condition);

        IQueryTextBuilder ReplacePrev(string value, TextType textType = TextType.Plain);

        IQueryTextBuilder ReplacePrev(Func<IText, IText> replacer);

        IQueryTextBuilder ReplacePrev(Func<string, string> replacer);

        IQueryTextBuilder StartBlock(string name);

        IQueryTextBuilder EndBlock();

        IQueryTextBuilder UseBlock(string name);
    }
}
