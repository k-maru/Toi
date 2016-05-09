using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Km.Toi.Template
{
    /// <summary>
    /// パラメーターの方向を定義します。
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// 入力パラメーターを表します。
        /// </summary>
        Input = 1,
        /// <summary>
        /// 出力パラメーターを表します。
        /// </summary>
        Output = 2,
        /// <summary>
        /// 入力および出力のパラメーターを表します。
        /// </summary>
        InputOutput = 3,
        /// <summary>
        /// ストアドプロシージャー等からの戻り値を表します。
        /// </summary>
        ReturnValue = 6
    }
}
