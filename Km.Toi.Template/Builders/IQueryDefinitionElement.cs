using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    public interface IQueryDefinitionElement
    {
        QueryDefinitionElementType ElementType { get; }

    }

    public interface IQueryDefinitionTextElement : IQueryDefinitionElement, ITextHolder
    {
    }

    public interface IQueryDefinitionParameterElement : IQueryDefinitionElement {
    }


}
