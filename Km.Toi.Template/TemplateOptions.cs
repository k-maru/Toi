using System;
using System.Collections.Generic;
using System.Linq;
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

        public TemplateOptions SetParameterFormat(string parameterFormat)
        {
            return new TemplateOptions(parameterFormat);
        }
    }
}
