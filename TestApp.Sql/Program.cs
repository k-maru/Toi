using Km.Toi.Template;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Sql
{
    class Program
    {

        private const string ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True";

        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var engine = new QueryTemplateEngine();
                var def = await engine.ExecuteAsync(File.ReadAllText("Sql\\Select.tmpl.sql"), new SearchCriteria() {
                    Countries = new List<string>() { "UK"}
                });

                using(var connection = new SqlConnection(ConnectionString))
                using(var command = connection.CreateCommand())
                {
                    Console.WriteLine("==SQL");
                    Console.WriteLine(def.QueryText);

                    command.CommandText = def.QueryText;
                    command.Parameters.AddRange(def.Parameters.Select(p =>
                    {
                        return new SqlParameter(p.Name, p.Value);
                    }).ToArray());
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        Console.WriteLine();
                        Console.WriteLine("==Result");
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine($"{reader.GetInt32(0)}, {reader.GetString(1)}, {reader.GetString(2)}");
                        }
                    }
                }
            }).Wait();
 
            
        }
    }

    public class SearchCriteria
    {
        public string CategoryName { get; set; }

        public string CategoryDesc { get; set; }

        public string CompanyName { get; set; }

        public List<string> Countries { get; set; } = new List<string>();

        public double? FromPrice { get; set; }

        public double? ToPrice { get; set; }
    }
}
