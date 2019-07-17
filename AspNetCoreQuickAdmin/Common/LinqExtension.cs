using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Common
{
    public static class LinqExtension
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate,
            bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }


        /// <summary>
        /// 根据某个字段去重
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, K>(this IEnumerable<T> source, Func<T, K> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, K>(keySelector, EqualityComparer<K>.Default));
        }

        /// <summary>
        /// 根据某个字段去重，可指定 StringComparer.CurrentCultureIgnoreCase忽略大小写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, K>(this IEnumerable<T> source, Func<T, K> keySelector,
            IEqualityComparer<K> comparer)
        {
            return source.Distinct(new CommonEqualityComparer<T, K>(keySelector, comparer));
        }

    }


}
