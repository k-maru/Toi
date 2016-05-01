using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite.EF6;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Km.Toi.EntityFramework.Test.Model
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext(DbConnection connection): base(connection, true)
        {

        }

        public static NorthwindContext Create()
        {
            var factory = new SQLiteProviderFactory();
            var connection = factory.CreateConnection();
            connection.ConnectionString = "Data Source=Northwind.db";
            return new NorthwindContext(connection);
        }
        
        //public NorthwindContext(DbContextOptions options)
        //    : base(options)
        //{
        //}

        //public DbSet<Customer> Customers { get; set; }

        //public DbSet<Employee> Employees { get; set; }

        //public DbSet<Order> Orders { get; set; }

        //public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Product> Products { get; set; }

        //public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

    }
}
