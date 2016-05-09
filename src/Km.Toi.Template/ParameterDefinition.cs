using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template
{
    /// <summary>
    /// SQLパラメーターを定義します。
    /// </summary>
    public class ParameterDefinition
    {
        /// <summary>
        /// 指定された名前および値を利用した、新しいインスタンスを生成します。
        /// </summary>
        /// <param name="name">パラメーター名</param>
        /// <param name="value">パラメーターの値</param>
        public ParameterDefinition(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
        /// <summary>
        /// パラメーターの名前を取得または設定します。
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// パラメーターの値を取得または設定します。
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// <see cref="ParameterDefinition.Value"/> プロパティを表すために使用するデータベース型名を取得または設定します。
        /// </summary>
        public string DbType { get; set; }
        /// <summary>
        /// <see cref="ParameterDefinition.Value"/> プロパティを表すために使用する最大桁数を取得または設定します。
        /// </summary>
        public byte? Precision { get; set; }
        /// <summary>
        /// <see cref="ParameterDefinition.Value"/> が解決される、小数点以下の桁数を取得または設定します。
        /// </summary>
        public byte? Scale { get; set; }
        /// <summary>
        /// 列内のデータの最大サイズをバイト単位で取得または設定します。
        /// </summary>
        public int? Size { get; set; }
        /// <summary>
        /// パラメーターが null 値を受け取るかどうかを示す値を取得または設定します。
        /// </summary>
        public bool? IsNullable { get; set; }
        /// <summary>
        /// パラメーターが入力専用、出力専用、双方向、またはストアド プロシージャの戻り値パラメーターのいずれであるかを示す値を取得または設定します。
        /// </summary>
        public Direction? Direction { get; set; }
    }
}
