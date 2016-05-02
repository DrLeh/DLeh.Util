using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLeh.Util.Extensions
{
    public static class EnumerableExtensions
    {

        /// <summary>
        /// For the given expression, if All are true this returns true, if any, null, if none, false.
        /// This is useful for marking checkboxes with data from various sources
        /// </summary>
        /// <typeparam name="T">Type of enumerable</typeparam>
        /// <param name="coll">Data to check</param>
        /// <param name="exp">Expression to check for</param>
        /// <param name="dataNullValue">Value to return if the collection is null. Default is false</param>
        /// <returns></returns>
        public static bool? AllAnyOrNone<T>(this IEnumerable<T> coll, Func<T, bool> exp, bool? dataNullValue = false)
        {
            return coll == null ? dataNullValue : (coll.All(exp) ? true : coll.Any(exp) ? (bool?)null : false);
        }
    }
}
