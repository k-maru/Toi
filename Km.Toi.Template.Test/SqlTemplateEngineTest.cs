using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Km.Toi.Template.Test
{
    public class SqlTemplateEngineTest
    {
        [Fact]
        public async Task 実行1()
        {
            var engine = new SqlTemplateEngine();
            var definition = await engine.ExecuteAsync(File.ReadAllText("TestFiles\\Simple.tmpl.sql"), new User() { Name = "Foo" }).ConfigureAwait(false);
            var text = definition.QueryText;
        }

        [Fact]
        public async Task 実行2()
        {
            var engine = new SqlTemplateEngine();
            var definition = await engine.ExecuteAsync(File.ReadAllText("TestFiles\\Simple.tmpl.sql"), new User() { Name = "Foo" }).ConfigureAwait(false);
            var text = definition.QueryText;
        }

        [Fact]
        public async Task 実行3()
        {
            var engine = new SqlTemplateEngine();
            var definition = await engine.ExecuteAsync(File.ReadAllText("TestFiles\\Simple.tmpl.sql"), new User() { Name = "Foo" }).ConfigureAwait(false);
            var text = definition.QueryText;
        }

        [Fact]
        public async Task usingの指定()
        {
            var engine = new SqlTemplateEngine();
            var definition = await engine.ExecuteAsync(File.ReadAllText("TestFiles\\Simple-using.tmpl.sql"), new User() { Name = "Foo" }).ConfigureAwait(false);
            var text = definition.QueryText;
        }
    }

    public class User {

        public string Name { get; set; }
    }

}
