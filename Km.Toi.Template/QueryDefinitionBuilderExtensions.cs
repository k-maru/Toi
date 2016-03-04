using Km.Toi.Template.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Km.Toi.Template
{
    public static class QueryDefinitionBuilderExtensions
    {
        public static void ToParameter(this IQueryDefinitionBuilder self, string parameterName, object value)
        {
            self.Text.ReplacePrev(val =>
            {
                var target = val.TrimEnd();
                var parameterText = string.Format(self.Options.ParameterFormat, parameterName);
                if (target.EndsWith("'"))
                {
                    target = Regex.Replace(target, "\\'[^\\']+$", parameterText);
                }
                else
                {
                    target = Regex.Replace(target, "(\\(|\\s[^\\(\\s]+$", parameterText);
                }
                self.Parameter.Add(new ParameterDefinition(parameterName, value));
                return target;
            });
        }
    }
}
