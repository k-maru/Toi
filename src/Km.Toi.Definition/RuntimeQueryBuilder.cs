using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Definition
{
    public sealed class RuntimeQueryBuilder: IQueryBuilder
    {
        private RuntimeQueryBuilder()
        {

        }

        public static QueryDefinition Create<T>(string queryTemplate, T model)
        {

            return null;
        }
    }
}
