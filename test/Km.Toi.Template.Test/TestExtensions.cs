using Km.Toi.Template.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Km.Toi.Template.Test
{
    public static class TestExtensions
    {
        public static void CustomExtension(this IQueryDefinitionBuilder self, string message)
        {
            self.Text.Add(message);
        }
    }
}
