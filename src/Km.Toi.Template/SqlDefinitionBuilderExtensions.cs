using Km.Toi.Template.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Km.Toi.Template
{
    public static class SqlDefinitionBuilderExtensions
    {
        public static void ToParameter(this ISqlDefinitionBuilder self, 
            string parameterName, object value,
            string dbType = null, byte? precision = null,
            byte? scale = null, int? size = null, bool? isNullable = null)
        {
            self.Text.ReplacePrev(val =>
            {
                var target = val.TrimEnd();
                var parameterText = string.Format(self.Options.ParameterFormat, parameterName);
                if (target.EndsWith("'", StringComparison.InvariantCulture))
                {
                    target = Regex.Replace(target, "\'(\'{2}|[^\'])+?\'$", parameterText);
                }
                else
                {
                    target = Regex.Replace(target, "((?<=[\\s\\=\\<\\>\\(\\,])|^)[^\\s\\=\\<\\>\\(\\,]+$", parameterText);
                }
                self.Parameter.Add(new ParameterDefinition(parameterText, value)
                {
                    DbType = dbType, Precision = precision, Scale = scale, Size = size, IsNullable = isNullable
                });
                return target;
            });
        }

        public static void ToInParameter<T>(this ISqlDefinitionBuilder self,
            string parameterPrefix, IEnumerable<T> values,
            string dbType = null, byte? precision = null,
            byte? scale = null, int? size = null, bool? isNullable = null)
        {
            if (values == null || !values.Any()) throw new ArgumentNullException(nameof(values));

            var paramDefinitions = values.Select((v, i) =>
            {
                var parameterText = string.Format(self.Options.ParameterFormat, $"{parameterPrefix}{i}");
                return new ParameterDefinition(parameterText, v)
                {
                    DbType = dbType,
                    Precision = precision,
                    Scale = scale,
                    Size = size,
                    IsNullable = isNullable
                };
            });
            self.Text.ReplacePrev(v =>
            {
                return $"{v.Substring(0, v.LastIndexOf("(", StringComparison.InvariantCulture))}( {paramDefinitions.Select(p => p.Name).ConcatWith(", ")}";
            });
            foreach(var p in paramDefinitions)
            {
                self.Parameter.Add(p);
            }
        }
    }
}
