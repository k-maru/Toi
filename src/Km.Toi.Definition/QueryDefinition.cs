﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Definition
{
    public sealed class QueryDefinition
    {
        public QueryDefinition(string queryText)
        {
            this.QueryText = queryText;
        }

        public string QueryText { get; }

        public List<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>();
    }
}
