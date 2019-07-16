using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 数据类型转换
    /// </summary>
    public static class StringHelper
    {

        /// <summary>
        /// 转化为Byte型
        /// </summary>
        public static Byte ToByte(this string source)
        {
            Byte reValue;
            Byte.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        ///  转化为Short型
        /// </summary>
        public static short ToShort(this string source)
        {
            short reValue;
            short.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为Int16(short)型
        /// </summary>
        public static short ToInt16(this string source)
        {
            if (source.IndexOf(".") > 0) //有小数的话
            {
                decimal num;
                decimal.TryParse(source, out num);
                return Convert.ToInt16(num);
            }

            short reValue;
            short.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为int32型
        /// </summary>
        public static int ToInt32(this string source)
        {
            if (source.IndexOf(".") > 0) //有小数的话
            {
                decimal num;
                decimal.TryParse(source, out num);
                return Convert.ToInt32(num);
            }

            int reValue;
            Int32.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为int32型
        /// </summary>
        /// <param name="source">原数据</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int ToInt32(this string source, int defaultValue = 0)
        {
            if (source.IndexOf(".") > 0) //有小数的话
            {
                decimal num;
                decimal.TryParse(source, out num);
                return Convert.ToInt32(num);
            }

            int reValue;
            return Int32.TryParse(source, out reValue) ? reValue : defaultValue;
        }

        /// <summary>
        /// 转化为int64型
        /// </summary>
        public static long ToInt64(this string source)
        {
            if (source.IndexOf(".") > 0) //有小数的话
            {
                decimal num;
                decimal.TryParse(source, out num);
                return Convert.ToInt64(num);
            }

            long reValue;
            Int64.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为Float型
        /// </summary>
        public static float ToFloat(this string source)
        {
            float reValue;
            float.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为Double型
        /// </summary>
        public static Double ToDouble(this string source)
        {
            Double reValue;
            Double.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为decimal型
        /// </summary>
        public static decimal ToDecimal(this string source)
        {
            decimal reValue;
            decimal.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为datetime型
        /// </summary>
        public static DateTime ToDateTime(this string source)
        {
            DateTime reValue;
            DateTime.TryParse(source, out reValue);
            return reValue;
        }
    }
}
