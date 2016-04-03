using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Parser
{
    public class CSharpScriptCodeParser: BaseCSharpParser
    {
        public CSharpScriptCodeParser(string templateCode): base(templateCode)
        {

        }

        protected override ParseResult PrepareCodeFragment(string fragment, IEnumerable<string> usings)
        {
            return new ParseResult(fragment, usings);
        }
    }
}
