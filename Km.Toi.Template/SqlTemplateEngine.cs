using Km.Toi.Template.Parser;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Km.Toi.Template.Builders;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.IO;

namespace Km.Toi.Template
{
    public class SqlTemplateEngine
    {
        public async Task<QueryDefinition> ExecuteAsync<T>(string queryTemplate, T model)
        {
            return await ExecuteAsync(queryTemplate, model, TemplateOptions.Default);
        }

        public async Task<QueryDefinition> ExecuteAsync<T>(string queryTemplate, T model, TemplateOptions options)
        {
            string loc = typeof(T).GetTypeInfo().Assembly.Location;
            var parser = new CSharpScriptCodeParser(File.ReadAllText(queryTemplate));
            var scriptOptions = ScriptOptions.Default.AddReferences(
                 typeof(SqlTemplateEngine).GetTypeInfo().Assembly,
                 typeof(T).GetTypeInfo().Assembly
            ).AddImports("Km.Toi.Template");

            var global = new Globals<T>(model, new QueryDefinitionBuilder(options));
            //var script = CSharpScript.Create(parser.Parse(), options, typeof(Globals<T>), )
            await CSharpScript.EvaluateAsync(parser.Parse(), scriptOptions, global, typeof(Globals<T>)).ConfigureAwait(false);

            return global.Context.Builder.Build();
        }
    }

    public class Globals<T>
    {
        public Globals(T model, QueryDefinitionBuilder builder)
        {
            this.Context = new Context<T>(model, builder);
        }

        public Context<T> Context { get; }

    }

    public class Context<T>
    {
        public Context(T model, QueryDefinitionBuilder builder)
        {
            this.Model = model;
            this.Builder = builder;
        }
        public T Model { get; }

        public QueryDefinitionBuilder Builder { get; }
    }
}
