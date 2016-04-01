using Km.Toi.Template.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Km.Toi.Template.Test.Parser
{
    public class CSharpScriptCodeParserTest
    {
        [Fact]
        public void シンプルな読み込み()
        {
            var source = File.ReadAllText("TestFiles\\Simple.tmpl.sql");
            var result = new CSharpScriptCodeParser(source).Parse();
            Assert.Equal(File.ReadAllText("TestFiles\\Simple.code.txt"), result.Code);
        }

        [Fact]
        public void ブロックコメントのネスト()
        {
            var source = File.ReadAllText("TestFiles\\NestComment.tmpl.sql");
            var result = new CSharpScriptCodeParser(source).Parse();
            Assert.Equal(File.ReadAllText("TestFiles\\NestComment.code.txt"), result.Code);
        }

        [Fact]
        public void SQLの文字列内のコメントとエスケープ()
        {
            var source = File.ReadAllText("TestFiles\\Strings.tmpl.sql");
            var result = new CSharpScriptCodeParser(source).Parse();
            Assert.Equal(File.ReadAllText("TestFiles\\Strings.code.txt"), result.Code);
        }

        [Fact]
        public void エクスクラメーションによるコメント()
        {
            var source = File.ReadAllText("TestFiles\\Comment.tmpl.sql");
            var result = new CSharpScriptCodeParser(source).Parse();
            Assert.Equal(File.ReadAllText("TestFiles\\Comment.code.txt"), result.Code);
        }

        [Fact]
        public void iによるインポート()
        {
            var source = File.ReadAllText("TestFiles\\Simple-using.tmpl.sql");
            var result = new CSharpScriptCodeParser(source).Parse();
            Assert.Equal(File.ReadAllText("TestFiles\\Simple-using.code.txt"), result.Code);
            Assert.Equal(result.Imports.First(), "Km.Toi.Template.Test");
        }

        [Fact]
        public void ハテナによるSQL文字の展開()
        {
            var source = File.ReadAllText("TestFiles\\Question.tmpl.sql");
            var result = new CSharpScriptCodeParser(source).Parse();
            Assert.Equal(File.ReadAllText("TestFiles\\Question.code.txt"), result.Code);
        }
    }
}
