using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Km.Toi.Definition
{
    class CSharpScriptCodeBuilder : ICodeBuilder
    {
        public string Build(string template)
        {
            var reader = new LookAheadReader(new StringReader(template), 3);
            var stringBuilder = new StringBuilder();
            while (reader.Peek() > -1)
            {
                if (ReadLineCode(reader, stringBuilder))
                {
                    continue;
                }
                if (ReadBlockCode(reader, stringBuilder))
                {
                    continue;
                }
                ReadText(reader, stringBuilder);
            }
            return stringBuilder.ToString();
        }

        private bool ReadLineCode(LookAheadReader reader, StringBuilder stringBuilder)
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
            if (builder[0] == '!') //code comment
            {
                return true;
            }
            stringBuilder.Append(builder.ToString()).AppendLine();
            return true;
        }

        private bool ReadBlockCode(LookAheadReader reader, StringBuilder stringBuilder)
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
                    if(commentDepth == 0)
                    {
                        reader.Read();
                        break;
                    }
                    builder.Append(value);
                    value = (char)reader.Read();
                    commentDepth--;
                }
                else if(value == '/' && (char)reader.Peek() == '*')
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
            if (builder[0] == '!') //code comment
            {
                return true;
            }
            stringBuilder.Append(builder.ToString()).AppendLine();
            return true;
        }

        private bool ReadText(LookAheadReader reader, StringBuilder stringBuilder)
        {
            var builder = new StringBuilder();
            var inText = false;
            while (true)
            {
                var peek = (char)reader.Peek();
                var peek1 = (char)reader.Peek(1);

                if(peek == '\'')
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
            var text = builder.ToString().Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
            stringBuilder.Append($"Context.Text.Add(\"{text}\");").AppendLine();
            return true;
        }
    }
}
