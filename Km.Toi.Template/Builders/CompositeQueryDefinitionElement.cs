using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template.Builders
{
    class CompositeQueryDefinitionElement : IQueryDefinitionElement, ITextHolder, IParameterHolder
    {
        private IQueryDefinitionBuilder builder;
        private CompositeQueryDefinitionElement blockElement;
        private CompositeQueryDefinitionElement parent;
        private List<IQueryDefinitionElement> elements = new List<IQueryDefinitionElement>();

        internal CompositeQueryDefinitionElement(IQueryDefinitionBuilder builder)
        {
            this.builder = builder;
            this.Use = true;
        }

        public CompositeQueryDefinitionElement(IQueryDefinitionBuilder builder, CompositeQueryDefinitionElement parent, string name)
        {
            this.builder = builder;
            this.Name = name;
            this.parent = parent;
        }

        private bool HasBlock => blockElement != null;

        QueryDefinitionElementType IQueryDefinitionElement.ElementType { get; } = QueryDefinitionElementType.None;

        public string Name { get; }

        private bool Use { get; set; }

        public void Add(IQueryDefinitionElement element)
        {
            if (blockElement != null)
            {
                blockElement.Add(element);
                return;
            }
            elements.Add(element);
        }

        public IQueryDefinitionElement Prev(QueryDefinitionElementType elemType)
        {
            if (blockElement != null)
            {
                return blockElement.Prev(elemType);
            }
            return elements.LastOrDefault(e => e.ElementType == elemType);
        }

        public void RemovePrev(QueryDefinitionElementType elemType)
        {
            if (blockElement != null)
            {
                blockElement.RemovePrev(elemType);
            }
            else
            {
                var targetIndex = elements.FindLastIndex(e => e.ElementType == elemType);
                if (targetIndex < 0)
                {
                    return;
                }
                elements.RemoveAt(targetIndex);

            }
        }

        public void ReplacePrev(IQueryDefinitionElement element)
        {
            if (blockElement != null)
            {
                blockElement.ReplacePrev(element);
            }
            else
            {
                var targetIndex = elements.FindLastIndex(e => e.ElementType == element.ElementType);
                if (targetIndex < 0)
                {
                    return;
                }
                elements.RemoveAt(targetIndex);
                elements.Insert(targetIndex, element);
            }
        }


        public void StartBlock(string name)
        {
            if (blockElement != null)
            {
                blockElement.StartBlock(name);
                return;
            }
            blockElement = new CompositeQueryDefinitionElement(this.builder, this, name);
            this.elements.Add(blockElement);
        }

        public void EndBlock()
        {
            if (blockElement != null && blockElement.HasBlock)
            {
                blockElement.EndBlock();
                return;
            }
            blockElement = null;
        }

        public void UseBlock(string name)
        {
            this.GetRoot().SetUseBlock(name);
        }

        private void SetUseBlock(string name)
        {
            foreach (var elem in this.elements.Where(elem => elem is CompositeQueryDefinitionElement).Cast<CompositeQueryDefinitionElement>())
            {
                if (elem.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    elem.Use = true;
                }
                elem.SetUseBlock(name);
            }
        }

        private CompositeQueryDefinitionElement GetRoot()
        {
            if (this.parent != null)
            {
                return this.parent.GetRoot();
            }
            return this;
        }

        public string GetText()
        {
            if (!this.Use)
            {
                return string.Empty;
            }
            return this.elements
                .Where(e => e is ITextHolder)
                .Cast<ITextHolder>()
                .Select(e => e.GetText())
                .ConcatWith("");
        }

        public IEnumerable<ParameterDefinition> GetParameters()
        {
            if (!this.Use)
            {
                return Enumerable.Empty<ParameterDefinition>();
            }
            return this.elements
                .Where(e => e is IParameterHolder)
                .Cast<IParameterHolder>()
                .SelectMany(e => e.GetParameters());
        }
    }
}
