using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template
{
    /// <summary>
    /// SQL文およびパラメーターを定義します。
    /// </summary>
    public sealed class SqlDefinition
    {
        /// <summary>
        /// 指定されたSQL文を利用してインスタンスを生成します。
        /// </summary>
        /// <param name="sqlText">SQL文</param>
        public SqlDefinition(string sqlText)
        {
            this.SqlText = sqlText;
        }
        /// <summary>
        /// SQL文を取得または設定します。
        /// </summary>
        public string SqlText { get; set; }
        /// <summary>
        /// パラメーターのリストを取得します。
        /// </summary>
        public List<ParameterDefinition> Parameters { get; } = new List<ParameterDefinition>();
    }
}
