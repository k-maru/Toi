using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Km.Toi.Template.Utilities
{
    static class IEnumerableExtension
    {
        /// <summary>
        /// シーケンスが <see cref="null"/> もしくは要素が含まれていないかどうかを判断します。
        /// </summary>
        /// <typeparam name="T">コレクションのタイプ</typeparam>
        /// <param name="source">シーケンス</param>
        /// <returns>シーケンスが <see cref="null"/> もしくは要素が含まれていない場合は true , そうでない場合は false</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source != null && source.Any();
        
        /// <summary>
        /// 指定されたシーケンスが null だった場合に、空のシーケンスを返します。
        /// </summary>
        /// <typeparam name="T">コレクションのタイプ</typeparam>
        /// <param name="source">シーケンス</param>
        /// <returns>指定されたシーケンスが null だった場合は空のシーケンス、そうでない場合は指定されたシーケンスのインスタンス</returns>
        public static IEnumerable<T> ToSafe<T>(this IEnumerable<T> source) => source == null ? Enumerable.Empty<T>() : source;


        public static string ConcatWith<T>(this IEnumerable<T> source, string separator)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return string.Join(separator, source);
        }

        public static string ConcatWith<T>(this IEnumerable<T> source, string separator, string format, IFormatProvider formatProvider = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (format == null) throw new ArgumentNullException(nameof(format));
            if (formatProvider != null)
            {
                return source.Select(s => string.Format(formatProvider, format, s)).ConcatWith(separator);
            }
            return source.Select(s => string.Format(format, s)).ConcatWith(separator);
        }
    }
}
