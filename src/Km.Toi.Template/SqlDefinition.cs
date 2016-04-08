using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template
{
    public sealed class SqlDefinition
    {
        public SqlDefinition(string sqlText)
        {
            this.SqlText = sqlText;
        }

        public string SqlText { get; set; }

        public List<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>();
    }
}
