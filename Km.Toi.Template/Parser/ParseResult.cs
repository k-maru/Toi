using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Km.Toi.Template.Parser
{
    public class ParseResult
    {
        public ParseResult(string code, IEnumerable<string> imports)
        {
            this.Code = code;
            this.Imports = imports;
        }

        public string Code { get; }

        public IEnumerable<string> Imports { get; }
    }
}
