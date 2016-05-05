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
using Km.Toi.Template.Utilities;

namespace Km.Toi.Template
{
    public class SqlTemplateEngine
    {
        private static ConcurrentDictionary<Tuple<string, Type>, ScriptRunner<object>> pool = new ConcurrentDictionary<Tuple<string, Type>, ScriptRunner<object>>();

        public async Task<SqlDefinition> ExecuteAsync(string sqlTemplate, object model) =>
            await ExecuteAsync(sqlTemplate, model, TemplateOptions.Default);

        public async Task<SqlDefinition> ExecuteAsync(string sqlTemplate, object model, TemplateOptions options)
        {
            Throws.NotEmpty(sqlTemplate, nameof(sqlTemplate));
            Throws.NotNull(model, nameof(model));

            if(options == null)
            {
                options = TemplateOptions.Default;
            }

            var modelType = model.GetType();
            
            ValidateModel(modelType, model);

            var globalsType = typeof(Globals<>).MakeGenericType(modelType);

            var runner = pool.GetOrAdd(Tuple.Create(sqlTemplate, modelType), v =>
            {
                var parser = new CSharpScriptCodeParser(sqlTemplate);
                var parseResult = parser.Parse();
                var scriptOptions = ScriptOptions.Default.AddReferences(
                     typeof(SqlTemplateEngine).GetTypeInfo().Assembly,
                     modelType.GetTypeInfo().Assembly
                ).AddImports("Km.Toi.Template", "System", "System.Linq").AddImports(parseResult.Imports);

                return CSharpScript.Create(parseResult.Code, scriptOptions, globalsType).CreateDelegate();
            });

            var global = Activator.CreateInstance(globalsType, model, new SqlDefinitionBuilder(options)) as IGlobals;
            await runner(global);
            return global.Builder.Build();
        }

        public async Task<SqlDefinition> ExecuteAsync(string sqlTemplate) =>
            await ExecuteAsync(sqlTemplate, TemplateOptions.Default);

        public async Task<SqlDefinition> ExecuteAsync(string sqlTemplate, TemplateOptions options) => await ExecuteAsync(sqlTemplate, new EmptyTemplateParameterModel(), options);
    

        private void ValidateModel(Type modelType, object model)
        {
            if (!modelType.IsPublic)
            {
                throw new ArgumentException(Resource.ModelIsMustBePublic, nameof(model));
            }
        }
    }

    public interface IGlobals {

        SqlDefinitionBuilder Builder { get; }
    }


    public class Globals<T>: IGlobals
    {
        public Globals(T model, SqlDefinitionBuilder builder)
        {
            this.Model = model;
            this.Builder = builder;
        }

        public T Model { get; }

        public SqlDefinitionBuilder Builder { get; }

    }

    public class EmptyTemplateParameterModel { }

}
