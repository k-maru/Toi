using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Km.Toi.Template
{
    public interface ITemplateOptions
    {
        string ParameterFormat { get; }
    }


    public interface ITemplateOptions<T> : ITemplateOptions where T: ITemplateOptions
    {
        T SetParameterFormat(string parameterFormat);
    }
}
