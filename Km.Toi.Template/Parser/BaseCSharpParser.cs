using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Km.Toi.Template.Parser
{
    public abstract class BaseCSharpParser: IQueryTemplateParser
    {
        private string parsedCode = null;
        public BaseCSharpParser(string templateCode)
        {
            this.TemplateCode = templateCode;
        }

        protected string TemplateCode { get; }

        public ParseResult Parse()
        {
            var reader = new LookAheadReader(new StringReader(this.TemplateCode), 3);
            var result = new Result();
            while (reader.Peek() > -1)
            {
                if (ReadLineCode(reader, result))
                {
                    continue;
                }
                if (ReadBlockCode(reader, result))
                {
                    continue;
                }
                ReadText(reader, result);
            }
            var fragment = result.Texts.ToString();
            return PrepareCodeFragment(fragment, result.Usings);
        }

        protected abstract ParseResult PrepareCodeFragment(string fagment, IEnumerable<string> usings);

        private bool ReadLineCode(LookAheadReader reader, Result result)
        {
            var top2 = string.Concat((char)reader.Peek(), (char)reader.Peek(1));
            if (top2 != "--")
            {
                return false;
            }
            reader.Read();
            reader.Read();

            var builder = new StringBuilder();
            while (reader.Peek() > -1)
            {
                var value = (char)reader.Read();
                if (value == '\r' || value == '\n')
                {
                    if (value == '\r' && (char)reader.Peek() == '\n')
                    {
                        reader.Read();
                    }
                    break;
                }
                builder.Append(value);
            }
            if (builder.Length < 1)
            {
                return true;
            }
            PrepareCodeText(result, builder.ToString(), false);
            return true;
        }

        private bool ReadBlockCode(LookAheadReader reader, Result result)
        {
            var top2 = string.Concat((char)reader.Peek(), (char)reader.Peek(1));
            if (top2 != "/*")
            {
                return false;
            }
            reader.Read();
            reader.Read();
            var builder = new StringBuilder();
            var commentDepth = 0;
            while (reader.Peek() > -1)
            {
                var value = (char)reader.Read();

                if (value == '*' && (char)reader.Peek() == '/')
                {
                    if (commentDepth == 0)
                    {
                        reader.Read();
                        break;
                    }
                    builder.Append(value);
                    value = (char)reader.Read();
                    commentDepth--;
                }
                else if (value == '/' && (char)reader.Peek() == '*')
                {
                    builder.Append(value);
                    value = (char)reader.Read();
                    commentDepth++;
                }

                builder.Append(value);
            }
            if (builder.Length < 1)
            {
                return true;
            }
            PrepareCodeText(result, builder.ToString(), true);
            return true;
        }

        private void PrepareCodeText(Result result, string value, bool isBlock = false)
        {
            if (value.Length > 1 && Char.IsWhiteSpace(value[1]))
            {
                var first = value[0];
                if (first == '!') //comment
                {
                    if (isBlock)
                    {
                        result.Texts.Append($"Context.Builder.Text.Add(\"/*{ReplaceNewLineCodeToEscapeCode(value.Substring(1))}*/\");").AppendLine();
                        return;
                    }
                    else
                    {
                        result.Texts.Append($"Context.Builder.Text.Add(\"--{ReplaceNewLineCodeToEscapeCode(value.Substring(1))}\");").AppendLine();
                        return;
                    }
                }
                if (first == '+' && isBlock)
                {
                    //HINT句(Oracle/MySQL)
                    result.Texts.Append($"Context.Builder.Text.Add(\"/*{ReplaceNewLineCodeToEscapeCode(value)}*/\");").AppendLine();
                    return;
                }
                if(first == 'i')
                {
                    //using
                    result.Usings.Add(value.Substring(1).Trim());
                    return;
                }
            }
            result.Texts.Append(value).AppendLine();
        }

        private bool ReadText(LookAheadReader reader, Result result)
        {
            var builder = new StringBuilder();
            var inText = false;
            while (true)
            {
                var peek = (char)reader.Peek();
                var peek1 = (char)reader.Peek(1);

                if (peek == '\'')
                {
                    if (!inText)
                    {
                        inText = true;
                    }
                    else
                    {
                        if (peek1 != '\'')
                        {
                            inText = false;
                        }
                        else
                        {
                            builder.Append((char)reader.Read());
                        }
                    }
                }
                else
                {
                    if (!inText)
                    {
                        var top2 = string.Concat(peek, peek1);
                        if (top2 == "--" || top2 == "/*")
                        {
                            break;
                        }
                    }
                }
                builder.Append((char)reader.Read());
                if (reader.Peek() < 0)
                {
                    break;
                }
            }
            if (builder.Length < 0)
            {
                return true;
            }
            result.Texts.Append($"Context.Builder.Text.Add(\"{ReplaceNewLineCodeToEscapeCode(builder.ToString())}\");").AppendLine();
            return true;
        }

        private string ReplaceNewLineCodeToEscapeCode(string value)
        {
            return value.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
        }

        private class Result
        {
            public StringBuilder Texts { get; } = new StringBuilder();

            public List<string> Usings { get; } = new List<string>();
        }
    }
}
