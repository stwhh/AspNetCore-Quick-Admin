using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Entities
{
    public partial class AuditLog : FullAudited
    {
        #region Business Fields

        /// <summary>
        /// 日志类型，枚举LogType。1:审计日志 2:异常日志 3:业务日志
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 请求的url
        /// </summary>
        [StringLength(1000)]
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [StringLength(10000)]
        public string RequestParam { get; set; }

        /// <summary>
        /// 响应参数
        /// </summary>
        public string ResponseParam { get; set; }

        /// <summary>
        /// Service名称
        /// </summary>
        [StringLength(255)]
        public string ServiceName { get; set; }

        /// <summary>
        /// Action名称
        /// </summary>
        [StringLength(255)]
        public string ActionName { get; set; }

        /// <summary>
        /// IpAddr
        /// </summary>
        [StringLength(255)]
        public string Ip { get; set; }

        /// <summary>
        /// UserAgent
        /// </summary>
        [StringLength(1000)]
        public string UserAgent { get; set; }

        /// <summary>
        /// 持续时间，
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 异常内容
        /// </summary>
        public string ExceptionContent { get; set; }

        /// <summary>
        /// 自定义日志内容
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }

        #endregion
    }
}
