using Km.Toi.Template.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Km.Toi.Template.Test.Builders
{
    public class QueryDefinitionBuilderTest
    {
        [Fact]
        public void テキストを追加できる()
        {
            var builder = new QueryDefinitionBuilder();
            builder.Text.Add("SELECT * FROM FOO ")
                    .Text.Add("WHERE 1 = 1");
            var def = builder.Build();
            Assert.Equal("SELECT * FROM FOO WHERE 1 = 1", def.QueryText);
        }

        [Fact]
        public void Useされていないブロック内のテキストは無視される()
        {
            var builder = new QueryDefinitionBuilder();
            builder.Text.Add("SELECT * FROM FOO ")
                    .StartBlock("Block 1")
                        .Text.Add("WHERE 1 = 1 ")
                    .EndBlock()
                    .Text.Add("AND 2 = 2");
            var def = builder.Build();
            Assert.Equal("SELECT * FROM FOO AND 2 = 2", def.QueryText);
        }

        [Fact]
        public void Useされたブロック内のテキストは追加される()
        {
            var builder = new QueryDefinitionBuilder();
            builder.Text.Add("SELECT * FROM FOO ")
                    .StartBlock("Block 1")
                        .Text.Add("WHERE 1 = 1 ")
                    .EndBlock()
                    .Text.Add("AND 2 = 2")
                    .UseBlock("Block 1");

            var def = builder.Build();
            Assert.Equal("SELECT * FROM FOO WHERE 1 = 1 AND 2 = 2", def.QueryText);
        }

        [Fact]
        public void パラメーターを追加できる()
        {
            var builder = new QueryDefinitionBuilder();
            builder.Text.Add("SELECT * FROM FOO ")
                .Text.Add("WHERE A = @A").Parameter.Add("A", 1);

            var def = builder.Build();
            Assert.Equal(1, def.Parameters.Count);
            Assert.Equal("A", def.Parameters[0].Name);
            Assert.Equal(1, def.Parameters[0].Value);
        }

        [Fact]
        public void Useされていないブロック内のパラメーターは無視される()
        {
            var builder = new QueryDefinitionBuilder();
            builder.Text.Add("SELECT * FROM FOO ")
                    .StartBlock("Block 1")
                        .Text.Add("WHERE A = @A ").Parameter.Add("A", 1)
                    .EndBlock()
                    .Text.Add("AND B = @B").Parameter.Add("B", 2);
            var def = builder.Build();
            Assert.Equal(1, def.Parameters.Count);
            Assert.Equal("B", def.Parameters[0].Name);
            Assert.Equal(2, def.Parameters[0].Value);
        }

        [Fact]
        public void Useされたブロック内のパラメーターは追加される()
        {
            var builder = new QueryDefinitionBuilder();
            builder.Text.Add("SELECT * FROM FOO ")
                    .StartBlock("Block 1")
                        .Text.Add("WHERE A = @A ").Parameter.Add("A", 1)
                    .EndBlock()
                    .Text.Add("AND B = @B").Parameter.Add("B", 2)
                    .UseBlock("Block 1");

            var def = builder.Build();
            Assert.Equal(2, def.Parameters.Count);
            Assert.Equal("A", def.Parameters[0].Name);
            Assert.Equal(1, def.Parameters[0].Value);
            Assert.Equal("B", def.Parameters[1].Name);
            Assert.Equal(2, def.Parameters[1].Value);
        }

        [Fact]
        public void ブロックのネストができる()
        {
            var builder = new QueryDefinitionBuilder();
            builder.Text.Add("SELECT * FROM FOO ")
                    .StartBlock("Block 1")
                        .Text.Add("WHERE 1 = 1 ")
                        .StartBlock("Block 1-1")
                            .Text.Add("AND 2 = 2 ")
                        .EndBlock()
                    .EndBlock()
                    .Text.Add("AND 3 = 3")
                    .UseBlock("Block 1").UseBlock("Block 1-1");

            var def = builder.Build();
            Assert.Equal("SELECT * FROM FOO WHERE 1 = 1 AND 2 = 2 AND 3 = 3", def.QueryText);
        }

        [Fact]
        public void 親のブロックがUseされていない場合は子のブロックは利用されない()
        {
            var builder = new QueryDefinitionBuilder();
            builder.Text.Add("SELECT * FROM FOO ")
                    .StartBlock("Block 1")
                        .Text.Add("WHERE 1 = 1 ")
                        .StartBlock("Block 1-1")
                            .Text.Add("AND 2 = 2 ")
                        .EndBlock()
                    .EndBlock()
                    .Text.Add("AND 3 = 3")
                    .UseBlock("Block 1-1");
            
            var def = builder.Build();
            Assert.Equal("SELECT * FROM FOO AND 3 = 3", def.QueryText);
        }

        [Fact]
        public void 子のブロックがUseされていない場合は子のブロックは利用されない()
        {
            var builder = new QueryDefinitionBuilder();
            builder.Text.Add("SELECT * FROM FOO ")
                    .StartBlock("Block 1")
                        .Text.Add("WHERE 1 = 1 ")
                        .StartBlock("Block 1-1")
                            .Text.Add("AND 2 = 2 ")
                        .EndBlock()
                    .EndBlock()
                    .Text.Add("AND 3 = 3")
                    .UseBlock("Block 1");

            var def = builder.Build();
            Assert.Equal("SELECT * FROM FOO WHERE 1 = 1 AND 3 = 3", def.QueryText);
        }

        [Fact]
        public void 同じ名前のブロックは一度にUseされる()
        {
            var builder = new QueryDefinitionBuilder();
            builder.Text.Add("SELECT * FROM FOO ")
                    .StartBlock("Block 1")
                        .Text.Add("WHERE 1 = 1 ")
                        .StartBlock("Block 1-1")
                            .Text.Add("AND 2 = 2 ")
                        .EndBlock()
                    .EndBlock()
                    .Text.Add("AND 3 = 3 ")
                    .Text.Add("AND EXISTS SELECT 1 FROM BAR WHERE BAR.A = FOO.A ")
                    .StartBlock("Block 1")
                        .Text.Add("AND BAR = 1")
                    .EndBlock()
                    .UseBlock("Block 1");

            var def = builder.Build();
            Assert.Equal("SELECT * FROM FOO WHERE 1 = 1 AND 3 = 3 AND EXISTS SELECT 1 FROM BAR WHERE BAR.A = FOO.A AND BAR = 1" , def.QueryText);
        }

        [Fact]
        public void 存在しないブロック名をUseされても無視される()
        {
            var builder = new QueryDefinitionBuilder();
            builder.Text.Add("SELECT * FROM FOO ")
                    .StartBlock("Block 1")
                        .Text.Add("WHERE 1 = 1 ")
                    .EndBlock()
                    .Text.Add("AND 2 = 2")
                    .UseBlock("Miss!!");

            var def = builder.Build();
            Assert.Equal("SELECT * FROM FOO AND 2 = 2", def.QueryText);
        }

        [Fact]
        public void 直前に設定されたテキストを取得できる()
        {
            var builder = new QueryDefinitionBuilder();
            builder.Text.Add("SELECT * FROM FOO ")
                .Text.Add("WHERE A = A");

            Assert.Equal("WHERE A = A", builder.Text.Prev());
        }

        [Fact]
        public void 直前に設定されたテキストがない場合はnullが返る()
        {
            var builder = new QueryDefinitionBuilder();

            Assert.Null(builder.Text.Prev());
        }

        [Fact]
        public void 直前のテキスト取得はBlock事となる()
        {
            var builder = new QueryDefinitionBuilder();

            builder.StartBlock("Block1");
            Assert.Null(builder.Text.Prev());
            builder.Text.Add("Foo");
            Assert.Equal("Foo", builder.Text.Prev());
            builder.EndBlock();
            Assert.Null(builder.Text.Prev());
            builder.Text.Add("Bar");
            Assert.Equal("Bar", builder.Text.Prev());

        }

        [Fact]
        public void 直前のテキストを削除できる()
        {
            var builder = new QueryDefinitionBuilder();

            builder.StartBlock("Block1");
            builder.Text.Add("Foo");
            Assert.Equal("Foo", builder.Text.Prev());
            builder.Text.RemovePrev();
            Assert.Null(builder.Text.Prev());
            builder.Text.Add("Bar");
            Assert.Equal("Bar", builder.Text.Prev());
            builder.EndBlock();
            Assert.Null(builder.Text.Prev());
            builder.Text.Add("Baz");
            Assert.Equal("Baz", builder.Text.Prev());
            builder.UseBlock("Block1");

            Assert.Equal("BarBaz", builder.Build().QueryText);
        }

        [Fact]
        public void 直前のテキストを置き換えられる()
        {
            var builder = new QueryDefinitionBuilder();

            builder.StartBlock("Block1");
            builder.Text.Add("Foo");
            Assert.Equal("Foo", builder.Text.Prev());
            builder.Text.ReplacePrev(v => "Foo1");
            Assert.Equal("Foo1", builder.Text.Prev());
            builder.Text.Add("Bar");
            Assert.Equal("Bar", builder.Text.Prev());
            builder.EndBlock();
            Assert.Null(builder.Text.Prev());
            builder.Text.Add("Baz");
            Assert.Equal("Baz", builder.Text.Prev());
            builder.Text.ReplacePrev(v => "Baz1");
            Assert.Equal("Baz1", builder.Text.Prev());

            builder.UseBlock("Block1");

            Assert.Equal("Foo1BarBaz1", builder.Build().QueryText);
        }
    }
}
