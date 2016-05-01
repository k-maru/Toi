using Km.Toi.EntityFramework.Test.Model;
using System;
using System.Collections.Generic;
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
            var context = NorthwindContext.Create();
            //context.Database.CreateIfNotExists();
            NorthwindData.CreateProducts(context);
        }

        [Fact]
        public async void Test()
        {
            using(var context = NorthwindContext.Create())
            {
                var tmplSql = new EntityFrameworkSql(context);
                var result = await tmplSql.QueryAsync<Product, Empty>("TestFiles\\SelectAllProduct.tmpl.sql", new Empty());
            }
        }

    }

    public class Empty
    {

    }
}
