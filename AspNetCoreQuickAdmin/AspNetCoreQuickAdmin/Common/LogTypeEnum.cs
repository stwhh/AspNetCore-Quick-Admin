using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreQuickAdmin.Common
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 普通的审计日志(访问日志)
        /// </summary>
        [Description("Audit")]
        Audit = 1,

        /// <summary>
        /// 异常日志
        /// </summary>
        [Description("Exception")]
        Exception = 2,

        /// <summary>
        /// 业务日志
        /// </summary>
        [Description("Business")]
        Business = 3
    }
}