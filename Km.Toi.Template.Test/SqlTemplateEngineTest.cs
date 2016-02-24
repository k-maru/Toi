using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Km.Toi.Template.Test
{
    public class SqlTemplateEngineTest
    {
        [Fact]
        public async Task 実行()
        {
            var engine = new SqlTemplateEngine();
            var definition = await engine.ExecuteAsync("TestFiles\\Simple.tmpl.sql", new User() { Name = "Foo" }).ConfigureAwait(false);
            var text = definition.QueryText;
        }
    }

    public class User {

        public string Name { get; set; }
    }

}
