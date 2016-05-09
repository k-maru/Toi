using Km.Toi.Template.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Km.Toi.Template.Test
{
    public class SqlDefinitionBuilderExtensionsTest
    {
        [Fact]
        public void ToParameterで値をパラメーターに変換できる()
        {
            var builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("123");
            builder.ToParameter("Num", 123);
            var def = builder.Build();
            Assert.Equal("@Num", def.SqlText);
            Assert.Equal(1, def.Parameters.Count);
            Assert.Equal(123, def.Parameters[0].Value);

            builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("Foo = 1");
            builder.ToParameter("Foo", 123);

            def = builder.Build();
            Assert.Equal("Foo = @Foo", def.SqlText);
            Assert.Equal(1, def.Parameters.Count);
            Assert.Equal(123, def.Parameters[0].Value);

            builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("Foo=1");
            builder.ToParameter("Foo", 123);

            def = builder.Build();
            Assert.Equal("Foo=@Foo", def.SqlText);
            Assert.Equal(1, def.Parameters.Count);
            Assert.Equal(123, def.Parameters[0].Value);

            builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("Foo>1");
            builder.ToParameter("Foo", 123);

            def = builder.Build();
            Assert.Equal("Foo>@Foo", def.SqlText);
            Assert.Equal(1, def.Parameters.Count);
            Assert.Equal(123, def.Parameters[0].Value);

            builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("Foo<1");
            builder.ToParameter("Foo", 123);

            def = builder.Build();
            Assert.Equal("Foo<@Foo", def.SqlText);
            Assert.Equal(1, def.Parameters.Count);
            Assert.Equal(123, def.Parameters[0].Value);

            builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("Foo=FUNC(1");
            builder.ToParameter("Foo", 123);
            builder.Text.Add(")");
            def = builder.Build();
            Assert.Equal("Foo=FUNC(@Foo)", def.SqlText);
            Assert.Equal(1, def.Parameters.Count);
            Assert.Equal(123, def.Parameters[0].Value);

            builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("Foo=FUNC(1,2");
            builder.ToParameter("Foo", 123);
            builder.Text.Add(")");
            def = builder.Build();
            Assert.Equal("Foo=FUNC(1,@Foo)", def.SqlText);
            Assert.Equal(1, def.Parameters.Count);
            Assert.Equal(123, def.Parameters[0].Value);

        }

        [Fact]
        public void ToParameterで文字列をパラメーターにできる()
        {
            var builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("'Foo'");
            builder.ToParameter("Foo", "ABC");
            var def = builder.Build();
            Assert.Equal("@Foo", def.SqlText);
            Assert.Equal(1, def.Parameters.Count);
            Assert.Equal("ABC", def.Parameters[0].Value);

            builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("'Fo''o'");
            builder.ToParameter("Foo", "ABC");
            def = builder.Build();
            Assert.Equal("@Foo", def.SqlText);
            Assert.Equal(1, def.Parameters.Count);
            Assert.Equal("ABC", def.Parameters[0].Value);

            builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("'Fo''''o'");
            builder.ToParameter("Foo", "ABC");
            def = builder.Build();
            Assert.Equal("@Foo", def.SqlText);
            Assert.Equal(1, def.Parameters.Count);
            Assert.Equal("ABC", def.Parameters[0].Value);

            builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("'F''o''o'");
            builder.ToParameter("Foo", "ABC");
            def = builder.Build();
            Assert.Equal("@Foo", def.SqlText);
            Assert.Equal(1, def.Parameters.Count);
            Assert.Equal("ABC", def.Parameters[0].Value);

        }

        [Fact]
        public void ToInParameterでIEnumerableをパラメーターにできる()
        {
            var builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("( 123, 456, 789 ");
            builder.ToInParameter("Foo", new[] { 111, 222, 333 });
            builder.Text.Add(" )");

            var def = builder.Build();
            Assert.Equal("( @Foo0, @Foo1, @Foo2 )", def.SqlText);
            Assert.Equal(3, def.Parameters.Count);
            Assert.Equal("Foo0", def.Parameters[0].Name);
            Assert.Equal(111, def.Parameters[0].Value);
            Assert.Equal("Foo1", def.Parameters[1].Name);
            Assert.Equal(222, def.Parameters[1].Value);
            Assert.Equal("Foo2", def.Parameters[2].Name);
            Assert.Equal(333, def.Parameters[2].Value);
        }

        [Fact]
        public void ToAutoNameParameterで自動的にパラメーター名が設定される()
        {
            var builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("123");
            builder.ToAutoNameParameter(123);
            builder.Text.Add("123");
            builder.ToAutoNameParameter("ABC");
            builder.Text.Add("'Foo'");
            builder.ToAutoNameParameter("ABC");
            builder.Text.Add("'Fo''o'");
            builder.ToAutoNameParameter("ABC");
            builder.Text.Add("'Fo''''o'");
            builder.ToAutoNameParameter("ABC");
            builder.Text.Add("Foo = 1");
            builder.ToAutoNameParameter(123);
            var def = builder.Build();
            Assert.Equal("@p0@p1@p2@p3@p4Foo = @p5", def.SqlText);
            Assert.Equal(6, def.Parameters.Count);
            Assert.Equal(123, def.Parameters[0].Value);
            Assert.Equal("ABC", def.Parameters[1].Value);
            Assert.Equal("ABC", def.Parameters[2].Value);
            Assert.Equal("ABC", def.Parameters[3].Value);
            Assert.Equal("ABC", def.Parameters[4].Value);
            Assert.Equal(123, def.Parameters[5].Value);
        }

        [Fact]
        public void ToAutoNameInParameterで自動的にパラメーター名が設定される()
        {
            var builder = new SqlDefinitionBuilder(TemplateOptions.Default);
            builder.Text.Add("( 123, 456, 789 ");
            builder.ToAutoNameInParameter(new[] { 111, 222, 333 });
            builder.Text.Add(" )");

            var def = builder.Build();
            Assert.Equal("( @p0_0, @p0_1, @p0_2 )", def.SqlText);
            Assert.Equal(3, def.Parameters.Count);
            Assert.Equal("p0_0", def.Parameters[0].Name);
            Assert.Equal(111, def.Parameters[0].Value);
            Assert.Equal("p0_1", def.Parameters[1].Name);
            Assert.Equal(222, def.Parameters[1].Value);
            Assert.Equal("p0_2", def.Parameters[2].Name);
            Assert.Equal(333, def.Parameters[2].Value);
        }
    }
}
