using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Km.Toi.Template
{
    public sealed class TemplateOptions : ITemplateOptions<TemplateOptions>
    {
        public readonly static TemplateOptions Default = new TemplateOptions("@{0}");

        private TemplateOptions(string parameterFormat)
        {
            this.ParameterFormat = parameterFormat;
        }

        public string ParameterFormat { get; }

        
        public TemplateOptions SetParameterFormat(string parameterFormat) =>
            new TemplateOptions(parameterFormat);
    }
}
