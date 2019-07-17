using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 重写默认比较器
    /// </summary>
    /// <typeparam name="T">比较类型</typeparam>
    /// <typeparam name="K">比较字段</typeparam>
    public class CommonEqualityComparer<T, K> : IEqualityComparer<T>
    {
        private readonly Func<T, K> _keySelector;
        private readonly IEqualityComparer<K> _comparer;

        public CommonEqualityComparer(Func<T, K> keySelector)
            : this(keySelector, EqualityComparer<K>.Default)
        {
        }

        public CommonEqualityComparer(Func<T, K> keySelector, IEqualityComparer<K> comparer)
        {
            _keySelector = keySelector;
            _comparer = comparer;
        }


        public bool Equals(T x, T y)
        {
            return _comparer.Equals(_keySelector(x), _keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return _comparer.GetHashCode(_keySelector(obj));
        }
    }
}
