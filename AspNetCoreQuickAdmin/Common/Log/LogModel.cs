using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Log
{
    public class LogModel
    {
        /// <summary>
        /// 日志类型，对应的枚举是 LogType
        /// 1:审计日志 2:异常日志 3:业务日志
        /// </summary>
        /// <remarks>注意，如果是Business日志只需传UserName,ExtraContent 俩个参数即可.其它参数是audit和error 用的
        ///  </remarks>
        public int LogType { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string RequestParam { get; set; }

        /// <summary>
        /// 响应参数
        /// </summary>
        public string ResponseParam { get; set; }

        /// <summary>
        /// Service名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Action名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddr { get; set; }

        /// <summary>
        /// 客户端信息
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 请求响应时间
        /// </summary>
        public int DurationTimeOfMillisecond { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 日志自定义内容
        /// </summary>
        public string ExtraContent { get; set; }

        public LogModel()
        {
            RequestTime = DateTime.Now;
        }
    }

}
