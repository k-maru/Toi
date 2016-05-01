using Km.Toi.Template;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Km.Toi.EntityFramework
{
    public sealed class EntityFrameworkSql
    {
        public EntityFrameworkSql(DbContext context)
        {
            this.DbContext = context;
        }

        public DbContext DbContext { get; }

        public async Task<IEnumerable<TResult>> QueryAsync<TResult, TModel>(string template, TModel model)
        {
            var engine = new SqlTemplateEngine();
            var definition = await engine.ExecuteAsync(File.ReadAllText(template), model);

            var tempCommand = this.DbContext.Database.Connection.CreateCommand();
            var parameters = definition.Parameters.Select(p => MapDefinitionToDbParameter(p, tempCommand));
            tempCommand.Parameters.Clear();
            tempCommand.Connection = null;

            return await this.DbContext.Database.SqlQuery<TResult>(definition.SqlText, parameters).ToListAsync();
        }

        public async Task<int> ExecuteCommandAsync<TModel>(string template, TModel model)
        {
            var engine = new SqlTemplateEngine();
            var definition = await engine.ExecuteAsync(File.ReadAllText(template), model);

            var tempCommand = this.DbContext.Database.Connection.CreateCommand();
            var parameters = definition.Parameters.Select(p => MapDefinitionToDbParameter(p, tempCommand));
            tempCommand.Parameters.Clear();
            tempCommand.Connection = null;

            return await this.DbContext.Database.ExecuteSqlCommandAsync(definition.SqlText, parameters);
        }

        private DbParameter MapDefinitionToDbParameter(ParameterDefinition def, DbCommand command)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = def.Name;
            parameter.Value = def.Value;
            parameter.DbType = MapDbTypeFromTypeName(def.DbType, parameter.DbType);
            parameter.Direction = MapParameterDirectionFromDirection(def.Direction, parameter.Direction);
            parameter.Precision = def.Precision ?? parameter.Precision;
            parameter.Scale = def.Scale ?? parameter.Scale;
            parameter.Size = def.Size ?? parameter.Size;
            return parameter;
        }

        private DbType MapDbTypeFromTypeName(string dbTypeName, DbType defaultType)
        {
            if (dbTypeName == DbTypeName.AnsiString) return DbType.AnsiString;
            if (dbTypeName == DbTypeName.AnsiStringFixedLength) return DbType.AnsiStringFixedLength;
            if (dbTypeName == DbTypeName.Binary) return DbType.Binary;
            if (dbTypeName == DbTypeName.Boolean) return DbType.Boolean;
            if (dbTypeName == DbTypeName.Byte) return DbType.Byte;
            if (dbTypeName == DbTypeName.Currency) return DbType.Currency;
            if (dbTypeName == DbTypeName.Date) return DbType.Date;
            if (dbTypeName == DbTypeName.DateTime) return DbType.DateTime;
            if (dbTypeName == DbTypeName.DateTime2) return DbType.DateTime2;
            if (dbTypeName == DbTypeName.DateTimeOffset) return DbType.DateTimeOffset;
            if (dbTypeName == DbTypeName.Decimal) return DbType.Decimal;
            if (dbTypeName == DbTypeName.Double) return DbType.Double;
            if (dbTypeName == DbTypeName.Guid) return DbType.Guid;
            if (dbTypeName == DbTypeName.Int16) return DbType.Int16;
            if (dbTypeName == DbTypeName.Int32) return DbType.Int32;
            if (dbTypeName == DbTypeName.Int64) return DbType.UInt64;
            if (dbTypeName == DbTypeName.Object) return DbType.Object;
            if (dbTypeName == DbTypeName.SByte) return DbType.SByte;
            if (dbTypeName == DbTypeName.Single) return DbType.Single;
            if (dbTypeName == DbTypeName.String) return DbType.String;
            if (dbTypeName == DbTypeName.StringFixedLength) return DbType.StringFixedLength;
            if (dbTypeName == DbTypeName.Time) return DbType.Time;
            if (dbTypeName == DbTypeName.UInt16) return DbType.UInt16;
            if (dbTypeName == DbTypeName.UInt32) return DbType.UInt32;
            if (dbTypeName == DbTypeName.UInt64) return DbType.UInt64;
            if (dbTypeName == DbTypeName.VarNumeric) return DbType.VarNumeric;
            if (dbTypeName == DbTypeName.Xml) return DbType.Xml;

            return defaultType;

        }

        private ParameterDirection MapParameterDirectionFromDirection(Direction? direction, ParameterDirection defaultDirection)
        {
            if (!direction.HasValue)
            {
                return defaultDirection;
            }
            var value = direction.Value;
            if (value == Direction.Input) return ParameterDirection.Input;
            if (value == Direction.Output) return ParameterDirection.Output;
            if (value == Direction.InputOutput) return ParameterDirection.InputOutput;
            if (value == Direction.ReturnValue) return ParameterDirection.ReturnValue;
            return defaultDirection;
        }
    }
}
