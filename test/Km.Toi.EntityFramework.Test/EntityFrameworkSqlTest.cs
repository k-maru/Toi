using Km.Toi.EntityFramework.Test.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite.EF6;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Km.Toi.EntityFramework.Test
{
    public class EntityFrameworkSqlTest
    {
        public EntityFrameworkSqlTest()
        {
            var context = NorthwindContext.CreateSqlServer();
            NorthwindData.CreateProducts(context);
        }

        [Fact]
        public async Task 引数無しで実行できる()
        {
            using(var context = NorthwindContext.CreateSqlServer())
            {
                var tmplSql = new EntityFrameworkSql(context);
                var result = await tmplSql.QueryAsync<Product>("TestFiles\\SelectAllProduct.tmpl.sql");
                Assert.Equal(77, result.Count());
            }
        }

        [Fact]
        public async Task パラメーターを指定して実行できる()
        {
            using (var context = NorthwindContext.CreateSqlServer())
            {
                var tmplSql = new EntityFrameworkSql(context);
                var result = await tmplSql.QueryAsync<Product>("TestFiles\\SelectProductByName.tmpl.sql", 
                    new Product() { ProductName = "Queso" });
                Assert.Equal(2, result.Count());
            }
        }
    }
}
