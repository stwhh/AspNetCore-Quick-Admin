using System.ComponentModel.DataAnnotations;

namespace QuickAdmin.Model.Entities
{
    public partial class AuditLog : FullAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [StringLength(36)]
        public string Id { get; set; }

        /// <summary>
        /// 日志类型，枚举LogType。1:审计日志 2:异常日志 3:业务日志
        /// </summary>
        [Required]
        public int Type { get; set; }

        /// <summary>
        /// 请求的url
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string RequestUrl { get; set; } = string.Empty;

        /// <summary>
        /// 请求参数
        /// </summary>
        [Required]
        [StringLength(4000)]
        public string RequestParam { get; set; } = string.Empty;

        /// <summary>
        /// 响应参数
        /// </summary>
        [Required]
        [StringLength(4000)]
        public string ResponseParam { get; set; } = string.Empty;

        /// <summary>
        /// Service名称
        /// </summary>
        [Required]
        [StringLength(255)]
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Action名称
        /// </summary>
        [StringLength(255)]
        [Required]
        public string ActionName { get; set; } = string.Empty;

        /// <summary>
        /// IpAddr
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Ip { get; set; } = string.Empty;

        /// <summary>
        /// UserAgent
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string UserAgent { get; set; } = string.Empty;

        /// <summary>
        /// 持续时间，
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 异常内容
        /// </summary>
        [Required]
        [StringLength(4000)]
        public string ExceptionContent { get; set; } = string.Empty;

        /// <summary>
        /// 自定义日志内容
        /// </summary>
        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

    }
}
