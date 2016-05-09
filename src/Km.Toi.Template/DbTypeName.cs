using Km.Toi.Template.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Km.Toi.Template
{
    public static class DbTypeName
    {
        private const string DefaultType = "String";

        public readonly static string AnsiString = "AnsiString";
        public readonly static string Binary = "Binary";
        public readonly static string Byte = "Byte";
        public readonly static string Boolean = "Boolean";
        public readonly static string Currency = "Currency";
        public readonly static string Date = "Date";
        public readonly static string DateTime = "DateTime";
        public readonly static string Decimal = "Decimal";
        public readonly static string Double = "Double";
        public readonly static string Guid = "Guid";
        public readonly static string Int16 = "Int16";
        public readonly static string Int32 = "Int32";
        public readonly static string Int64 = "Int64";
        public readonly static string Object = "Object";
        public readonly static string SByte = "SByte";
        public readonly static string Single = "Single";
        public readonly static string String = DefaultType;
        public readonly static string Time = "Time";
        public readonly static string UInt16 = "UInt16";
        public readonly static string UInt32 = "UInt32";
        public readonly static string UInt64 = "UInt64";
        public readonly static string VarNumeric = "VerNumeric";
        public readonly static string AnsiStringFixedLength = "AnsiStringFixedLength";
        public readonly static string StringFixedLength = "StringFixedLength";
        public readonly static string Xml = "Xml";
        public readonly static string DateTime2 = "DateTime2";
        public readonly static string DateTimeOffset = "DateTimeOffset";

    }
}
