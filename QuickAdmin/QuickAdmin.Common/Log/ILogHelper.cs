namespace QuickAdmin.Common.Log
{
    public interface ILogHelper
    {
        /// <summary>
        /// 错误日志
        /// </summary>
        void ErrorAsync(LogModel model);

        /// <summary>
        /// 审计日志
        /// </summary>
        void AuditAsync(LogModel model);

        /// <summary>
        /// 业务日志
        /// </summary>
        void BusinessAsync(LogModel model);
    }
}