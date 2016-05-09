using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Km.Toi.Template
{
    /// <summary>
    /// テンプレートのオプション値を設定するインターフェイスを定義します。
    /// </summary>
    public interface ITemplateOptions
    {
        /// <summary>
        /// SQLのパラメーターのフォーマットを取得します。
        /// </summary>
        string ParameterFormat { get; }
    }

    /// <summary>
    /// テンプレートのオプション値を設定するインターフェイスを定義します。
    /// </summary>
    /// <typeparam name="T">テンプレートのオプション値を設定する実体型</typeparam>
    public interface ITemplateOptions<T> : ITemplateOptions where T: ITemplateOptions
    {
        /// <summary>
        /// SQLのパラメーターのフォーマットを設定し、新しいインスタンスを返します。
        /// </summary>
        /// <param name="parameterFormat">SQLのパラメーターのフォーマット</param>
        /// <returns>指定されたSQLのパラメーターのフォーマットが設定された新しいインスタンス</returns>
        T SetParameterFormat(string parameterFormat);
    }
}
