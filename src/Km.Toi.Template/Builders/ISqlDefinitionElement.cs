using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    public interface ISqlDefinitionElement
    {
        SqlDefinitionElementType ElementType { get; }

    }

    public interface ISqlDefinitionTextElement : ISqlDefinitionElement, ITextHolder
    {
    }

    public interface ISqlDefinitionParameterElement : ISqlDefinitionElement {
    }


}
