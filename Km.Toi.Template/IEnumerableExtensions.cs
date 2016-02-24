using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Km.Toi.Template
{
    public static class IEnumerableExtension
    {
        public static string ConcatWith<T>(this IEnumerable<T> source, string separator)
        {
            if (source == null) throw new ArgumentNullException("source");
            return string.Join(separator, source);
        }

        public static string ConcatWith<T>(this IEnumerable<T> source, string separator, string format, IFormatProvider formatProvider = null)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (format == null) throw new ArgumentNullException("format");
            if (formatProvider != null)
            {
                return source.Select(s => string.Format(formatProvider, format, s)).ConcatWith(separator);
            }
            return source.Select(s => string.Format(format, s)).ConcatWith(separator);
        }
    }
}
