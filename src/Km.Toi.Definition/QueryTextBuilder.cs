using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Definition
{
    public interface IText
    {
        string Value { get; }

        TextType TextType { get; }
    }


    interface ITextOrGroup : IText
    {
        bool IsGroup { get; }

        string Name { get; }

        bool Use { get; set; }
    }

    public enum TextType
    {
        Plain,
        BlockComment,
        LineComment,
        None
    }



    class Text : ITextOrGroup
    {
        private string value;
        public Text(string value, TextType textType = TextType.Plain)
        {
            this.value = value;
            this.TextType = textType;
        }

        public bool IsGroup { get; } = false;

        public string Name { get; } = null;

        public bool Use { get { return true; } set {; } }

        public string Value
        {
            get
            {
                return this.ToString();
            }
        }

        public override string ToString()
        {
            if (TextType == TextType.LineComment)
            {
                return $"--{this.value}{Environment.NewLine}";
            }
            if (TextType == TextType.LineComment)
            {
                return $"/*{this.value}*/{Environment.NewLine}";
            }
            if (TextType == TextType.Plain)
            {
                return this.value;
            }
            return null;
        }

        public TextType TextType { get; }
    }

    class GroupText : ITextOrGroup
    {
        public GroupText(string name, QueryTextBuilder builder)
        {
            this.Name = name;
            this.Builder = builder;
        }

        public bool IsGroup { get; } = true;

        public string Name { get; }

        public QueryTextBuilder Builder { get; }

        public bool Use { get; set; } = false;

        public string Value
        {
            get
            {
                return this.ToString();
            }
        }

        public override string ToString() => Builder.ToString();

        public TextType TextType
        {
            get { return TextType.None; }
        }
    }

    public class QueryTextBuilder : IQueryTextBuilder
    {
        private List<ITextOrGroup> texts = new List<ITextOrGroup>();
        private QueryTextBuilder parent = null;
        private QueryTextBuilder marker = null;

        public QueryTextBuilder() : this(null)
        {

        }

        private QueryTextBuilder(QueryTextBuilder parent)
        {
            this.parent = parent;
        }

        public IQueryTextBuilder Add(string value)
        {
            if (marker == null) texts.Add(new Text(value));
            else marker.Add(value);

            return this;
        }

        public IQueryTextBuilder AddIf(bool condition, string value)
        {
            if (condition)
            {
                Add(value);
            }

            return this;
        }

        public string GetPrev()
        {
            if (marker == null)
            {
                var last = texts.Where(t => !t.IsGroup).LastOrDefault();
                return last?.ToString();
            }
            else
            {
                return marker.GetPrev();
            }
        }

        public IQueryTextBuilder RemovePrev()
        {
            if (marker == null)
            {
                var last = texts.Where(t => !t.IsGroup).LastOrDefault();
                if (last != null)
                {
                    texts.Remove(last);
                }
            }
            else
            {
                marker.RemovePrev();
            }

            return this;
        }

        public IQueryTextBuilder RemovePrevIf(bool condition)
        {
            if (condition)
            {
                RemovePrev();
            }

            return this;
        }

        public IQueryTextBuilder ReplacePrev(string value, TextType textType = TextType.Plain)
        {
            if (marker == null)
            {
                var last = texts.Where(t => !t.IsGroup).LastOrDefault();
                if (last != null)
                {
                    var index = texts.IndexOf(last);
                    texts.RemoveAt(index);
                    texts.Insert(index, new Text(value, textType));
                }
            }
            else
            {
                marker.ReplacePrev(value);
            }

            return this;
        }

        public IQueryTextBuilder ReplacePrev(Func<IText, IText> replacer)
        {
            if (marker == null)
            {
                var last = texts.Where(t => !t.IsGroup).LastOrDefault() as Text;
                if (last != null)
                {
                    var value = replacer(last);
                    var textValue = new Text(value.Value, value.TextType);
                    var index = texts.IndexOf(last);
                    texts.RemoveAt(index);
                    texts.Insert(index, textValue);
                }
            }
            else
            {
                marker.ReplacePrev(replacer);
            }

            return this;
        }

        public IQueryTextBuilder ReplacePrev(Func<string, string> replacer)
        {
            if (marker == null)
            {
                var last = texts.Where(t => !t.IsGroup).LastOrDefault() as Text;
                if (last != null)
                {
                    var value = replacer(last.Value);
                    var textValue = new Text(value);
                    var index = texts.IndexOf(last);
                    texts.RemoveAt(index);
                    texts.Insert(index, textValue);
                }
            }
            else
            {
                marker.ReplacePrev(replacer);
            }

            return this;

        }

        public IQueryTextBuilder StartBlock(string name)
        {
            if (!this.GetRoot().CheckValidMarkerName(name))
            {
                throw new ArgumentException(string.Format(Resource.DuplicateBlockName, name), nameof(name));
            }
            if (marker == null)
            {
                marker = new QueryTextBuilder(this);
                texts.Add(new GroupText(name, marker));
            }
            else
            {
                marker.StartBlock(name);
            }

            return this;
        }

        public IQueryTextBuilder EndBlock()
        {
            if (marker != null && marker.HasMarker)
            {
                marker.EndBlock();
            }
            else
            {
                marker = null;
            }

            return this;
        }

        public IQueryTextBuilder UseBlock(string name)
        {
            if (!this.GetRoot().SetUseBlock(name))
            {
                throw new ArgumentException(string.Format(Resource.BlockNameNotFound, name), nameof(name));
            }
            return this;
        }

        public override string ToString()
        {
            return texts.Where(t => !t.IsGroup ? true : t.Use).ConcatWith("");
        }

        private bool HasMarker => marker != null;

        private QueryTextBuilder GetRoot()
        {
            if (this.parent == null)
            {
                return this;
            }
            return this.parent.GetRoot();
        }

        private bool CheckValidMarkerName(string name)
        {
            foreach (var group in texts.Where(t => t.IsGroup).Cast<GroupText>())
            {
                if (group.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return false;
                }
                if (!group.Builder.CheckValidMarkerName(name))
                {
                    return false;
                }
            }
            return true;
        }

        private bool SetUseBlock(string name)
        {
            foreach (var group in texts.Where(t => t.IsGroup).Cast<GroupText>())
            {
                if (group.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    group.Use = true;
                    return true;
                }
                if (group.Builder.SetUseBlock(name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
