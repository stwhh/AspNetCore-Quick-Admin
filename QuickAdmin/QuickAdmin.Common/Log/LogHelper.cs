using System;
using System.Threading.Tasks;
using QuickAdmin.DAO;
using QuickAdmin.DAO.Repository;
using QuickAdmin.Model.Entities;

namespace QuickAdmin.Common.Log
{
    public class LogHelper : ILogHelper
    {
        private readonly QuickAdminDbContext _context;
        private readonly IQuickAdminRepository<AuditLog> _logRepository;
        private const int LogContentMaxLength = 4000;

        public LogHelper(QuickAdminDbContext context, IQuickAdminRepository<AuditLog> logRepository)
        {
            _context = context;
            _logRepository = logRepository;
        }

        public void ErrorAsync(LogModel model)
        {
            Task.Run(() =>
            {
                var entity = GetLogEntity(model);
                //_logRepository.Insert(entity);
                _context.AuditLog.Add(entity);
                _context.SaveChanges();
            });
        }


        /// <summary>
        /// 审计日志
        /// </summary>
        /// <param name="model"></param>
        public void AuditAsync(LogModel model)
        {
            Task.Run(() =>
            {
                var entity = GetLogEntity(model);
                _context.AuditLog.Add(entity);
                _context.SaveChanges();
            });
        }

        /// <summary>
        /// 业务日志
        /// </summary>
        /// <param name="model"></param>
        public void BusinessAsync(LogModel model)
        {
            Task.Run(() =>
            {
                var entity = GetLogEntity(model);
                _context.AuditLog.Add(entity);
                _context.SaveChanges();
            });
        }

        /// <summary>
        /// 获取日志实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static AuditLog GetLogEntity(LogModel model)
        {
            var exceptionContent = model.Exception?.Message + model.Exception?.StackTrace;
            var entity = new AuditLog()
            {
                Id = Guid.NewGuid().ToString(),
                Type = model.LogType,
                RequestUrl = model.RequestUrl ?? string.Empty,
                RequestParam = model.RequestParam ?? string.Empty,
                ResponseParam = model.ResponseParam ?? string.Empty,
                ServiceName = model.ServiceName ?? string.Empty,
                ActionName = model.ActionName ?? string.Empty,
                Ip = model.IpAddr ?? string.Empty,
                UserAgent = model.UserAgent ?? string.Empty,
                Duration = model.DurationTimeOfMillisecond,
                ExceptionContent = (exceptionContent.Length > LogContentMaxLength
                                       ? exceptionContent.Substring(0, LogContentMaxLength)
                                       : exceptionContent) ?? string.Empty,
                Description = model.ExtraContent ?? string.Empty,
                CreateTime = model.RequestTime,
                CreateUserId = model.UserName ?? string.Empty
            };
            return entity;
        }
    }
}