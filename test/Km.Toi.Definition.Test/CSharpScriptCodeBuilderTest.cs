﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Km.Toi.Definition.Test
{
    public class CSharpScriptCodeBuilderTest
    {
        [Fact]
        public void シンプルな読み込み()
        {
            var source = File.ReadAllText("TestFiles\\Simple.tmpl.sql");
            var code = new CSharpScriptCodeBuilder().Build(source);
            Assert.Equal(File.ReadAllText("TestFiles\\Simple.code.txt"), code);
        }

        [Fact]
        public void ブロックコメントのネスト()
        {
            var source = File.ReadAllText("TestFiles\\NestComment.tmpl.sql");
            var code = new CSharpScriptCodeBuilder().Build(source);
            Assert.Equal(File.ReadAllText("TestFiles\\NestComment.code.txt"), code);
        }

        [Fact]
        public void SQLの文字列内のコメントとエスケープ()
        {
            var source = File.ReadAllText("TestFiles\\Strings.tmpl.sql");
            var code = new CSharpScriptCodeBuilder().Build(source);
            Assert.Equal(File.ReadAllText("TestFiles\\Strings.code.txt"), code);
        }
    }
}
