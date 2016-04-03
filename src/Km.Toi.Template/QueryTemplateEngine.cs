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
using System.Collections.Concurrent;

namespace Km.Toi.Template
{
    public class QueryTemplateEngine
    {
        private static ConcurrentDictionary<Tuple<string, Type>, ScriptRunner<object>> pool = new ConcurrentDictionary<Tuple<string, Type>, ScriptRunner<object>>(); 

        public async Task<QueryDefinition> ExecuteAsync<T>(string queryTemplate, T model)
        {
            return await ExecuteAsync(queryTemplate, model, TemplateOptions.Default);
        }

        public async Task<QueryDefinition> ExecuteAsync<T>(string queryTemplate, T model, TemplateOptions options)
        {
            var runner = pool.GetOrAdd(Tuple.Create(queryTemplate, typeof(T)), v =>
            {
                var parser = new CSharpScriptCodeParser(queryTemplate);
                var parseResult = parser.Parse();
                var scriptOptions = ScriptOptions.Default.AddReferences(
                     typeof(QueryTemplateEngine).GetTypeInfo().Assembly,
                     typeof(T).GetTypeInfo().Assembly
                ).AddImports("Km.Toi.Template", "System", "System.Linq").AddImports(parseResult.Imports);

                return CSharpScript.Create(parseResult.Code, scriptOptions, typeof(Globals<T>)).CreateDelegate();
            });

            var global = new Globals<T>(model, new QueryDefinitionBuilder(options));
            await runner(global);
            return global.Builder.Build();
        }
    }

    public class Globals<T>
    {
        public Globals(T model, QueryDefinitionBuilder builder)
        {
            this.Model = model;
            this.Builder = builder;
        }

        public T Model { get; }

        public QueryDefinitionBuilder Builder { get; }

    }
}
