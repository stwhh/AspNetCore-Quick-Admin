using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using QuickAdmin.Common.Log;

namespace QuickAdmin.Common.Filters
{
    /// <summary>
    /// AuditLogActionFilter，监控API访问以及性能
    /// </summary>
    public class AuditLogActionFilter : ActionFilterAttribute, IExceptionFilter
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogHelper _log;
        private bool _isIgnore;

        private readonly Stopwatch _timer = new Stopwatch();
        private LogModel _logModel;

        public AuditLogActionFilter(IHttpContextAccessor accessor, ILogHelper log)
        {
            _accessor = accessor;
            _log = log;
            _logModel = new LogModel();
        }

        /// <summary>
        /// 1、action执行前
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IsIgnore(context);

            if (_isIgnore)
            {
                return;
            }

            _timer.Start();

            GetRequestInfo((int) LogType.Audit, context);
        }

        /// <summary>
        /// 是否忽略，比如查看日志的操作没必要记录，否则响应参数会一层层嵌套
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public void IsIgnore(ActionExecutingContext context)
        {
            _isIgnore = context.ActionDescriptor.FilterDescriptors
                .Select(f => f.Filter)
                .OfType<TypeFilterAttribute>()
                .Any(f => f.ImplementationType == typeof(NotAuditLogActionFilter));
        }

        /// <summary>
        /// 获取请求信息【主要的信息都是在这里获取的】
        /// </summary>
        /// <param name="logType">日志类型枚举LogType。1:审计日志 2:异常日志 3:业务日志</param>
        /// <param name="context">ActionExecutingContext</param>
        public void GetRequestInfo(int logType, ActionExecutingContext context)
        {
            var request = _accessor.HttpContext.Request;

            _logModel.LogType = logType;
            //_logModel.UserName = _accessor.HttpContext.User.Identity.Name;
            //var allClaims = _accessor.HttpContext.User.Claims;
            //生成token用的Claim信息会加在token Payload里，这里都可以获取到
            var userName = _accessor.HttpContext.User.FindFirst("name")?.Value;
            var userNo = _accessor.HttpContext.User.FindFirst("userNo")?.Value;
            var role = _accessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var email = _accessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var issuer = _accessor.HttpContext.User.FindFirst("iss")?.Value;
            var audience = _accessor.HttpContext.User.FindFirst("aud")?.Value;

            _logModel.UserName = userName;
            _logModel.RequestUrl = request.GetDisplayUrl();
            _logModel.ServiceName = context.RouteData.Values["Controller"].ToString();
            _logModel.ActionName = context.RouteData.Values["Action"].ToString();
            var ipAddr = _accessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddr = request.Headers["X-Forwarded-For"];
            }

            _logModel.IpAddr = ipAddr;

            if (request.Headers.ContainsKey("User-Agent"))
            {
                var userAgent = request.Headers["User-Agent"].ToString();
                _logModel.UserAgent = userAgent;
            }

            var requestParms = JsonConvert.SerializeObject(context.ActionArguments); //包括参数名称也加到json文件里
            //  var s2 =JsonConvert.SerializeObject(context.ActionArguments.Values); //只有value，不包括参数名称，而且是数组形式

            _logModel.RequestParam = requestParms;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
        }

        /// <summary>
        /// 2、result执行后发生
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (_isIgnore)
            {
                return;
            }

            if (context.Result is JsonResult) //ObjectResult
            {
                _logModel.ResponseParam = JsonConvert.SerializeObject(((JsonResult) context.Result).Value);
            }

            _timer.Stop();
            _logModel.DurationTimeOfMillisecond = _timer.ElapsedMilliseconds.ToString().ToInt32();
            _log.AuditAsync(_logModel);
        }

        /// <summary>
        /// 3、发生异常时
        /// </summary>
        /// <param name="context">context</param>
        public void OnException(ExceptionContext context)
        {
            if (_isIgnore)
            {
                return;
            }

            _timer.Stop();
            _logModel.LogType = (int) LogType.Exception;
            _logModel.Exception = context.Exception;
            _logModel.DurationTimeOfMillisecond = _timer.ElapsedMilliseconds.ToString().ToInt32();
            _log.ErrorAsync(_logModel);

            context.HttpContext.Response.StatusCode = 500;
            context.HttpContext.Response.WriteAsync(context.Exception.Message);
            context.ExceptionHandled = true;
        }
    }
}