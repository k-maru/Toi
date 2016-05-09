using Km.Toi.Template.Builders;
using Km.Toi.Template.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Km.Toi.Template
{
    /// <summary>
    /// <see cref="ISqlDefinitionBuilder"/> に対する拡張メソッドを定義します。
    /// </summary>
    public static class SqlDefinitionBuilderExtensions
    {
        private static readonly string DefaultAutoParameterNameFormat = "p{0}";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <param name="size"></param>
        /// <param name="isNullable"></param>
        public static void ToAutoNameParameter(this ISqlDefinitionBuilder self,
            object value, string dbType = null, byte? precision = null,
            byte? scale = null, int? size = null, bool? isNullable = null) =>
                ToParameter(self, CreateAutoParameterName(self), value, dbType, precision, scale, size, isNullable);


        public static void ToParameter(this ISqlDefinitionBuilder self, 
            string parameterName, object value,
            string dbType = null, byte? precision = null,
            byte? scale = null, int? size = null, bool? isNullable = null)
        {
            Throws.NotEmpty(parameterName, nameof(parameterName));

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
                if (string.IsNullOrEmpty(dbType) && value != null)
                {
                    var map = DbTypeNameMap.Default();
                    var valueType = value.GetType();
                    if (map.ContainsKey(valueType))
                    {
                        dbType = map[valueType];
                    }
                }
                self.Parameter.Add(new ParameterDefinition(parameterName, value)
                {
                    DbType = dbType, Precision = precision, Scale = scale, Size = size, IsNullable = isNullable
                });
                return target;
            });
        }

        public static void ToAutoNameInParameter<T>(this ISqlDefinitionBuilder self,
            IEnumerable<T> values, string dbType = null, byte? precision = null,
            byte? scale = null, int? size = null, bool? isNullable = null) =>
                ToInParameter(self, CreateAutoParameterName(self) + "_", values, dbType, precision, scale, size, isNullable);

        public static void ToInParameter<T>(this ISqlDefinitionBuilder self,
            string parameterPrefix, IEnumerable<T> values,
            string dbType = null, byte? precision = null,
            byte? scale = null, int? size = null, bool? isNullable = null)
        {
            Throws.NotEmpty(parameterPrefix, nameof(parameterPrefix));

            if (values == null || !values.Any()) throw new ArgumentNullException(nameof(values));

            if (string.IsNullOrEmpty(dbType))
            {
                var map = DbTypeNameMap.Default();
                var valueType = typeof(T);
                if (map.ContainsKey(valueType))
                {
                    dbType = map[valueType];
                }
            }

            var paramDefinitions = values.Select((v, i) =>
            {
                var parameterName = $"{parameterPrefix}{i}";
                return new ParameterDefinition(parameterName, v)
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
                return $"{v.Substring(0, v.LastIndexOf("(", StringComparison.InvariantCulture))}( {paramDefinitions.Select(p => string.Format(self.Options.ParameterFormat, p.Name)).ConcatWith(", ")}";
            });
            foreach(var p in paramDefinitions)
            {
                self.Parameter.Add(p);
            }
        }

        private static string CreateAutoParameterName(ISqlDefinitionBuilder builder)
        {
            var names = builder.Parameter.GetNames().ToList();
            foreach (var index in Enumerable.Range(names.Count, int.MaxValue - names.Count))
            {
                var parameterName = string.Format(DefaultAutoParameterNameFormat, index);
                if (!names.Contains(parameterName))
                {
                    return parameterName;
                }
            }
            throw new SqlTemplateyException(Resource.CantCreateAutoParameterTooManyParameter);
        }
    }
}
