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
            var definition = await engine.ExecuteAsync(File.ReadAllText("TestFiles\\Simple.tmpl.sql"), 
                new User() { Name = "Foo" }).ConfigureAwait(false);
            var text = definition.SqlText;
        }

        [Fact]
        public async Task usingの指定()
        {
            var engine = new SqlTemplateEngine();
            var definition = await engine.ExecuteAsync(File.ReadAllText("TestFiles\\Simple-using.tmpl.sql"), 
                new User() { Name = "Foo" }).ConfigureAwait(false);
            var text = definition.SqlText;
        }
    }

    public class User {

        public string Name { get; set; }
    }

}
