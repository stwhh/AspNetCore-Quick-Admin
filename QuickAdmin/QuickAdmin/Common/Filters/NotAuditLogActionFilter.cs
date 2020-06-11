using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace QuickAdmin.Common.Filters
{
    /// <summary>
    /// 不需要记日志的特性
    /// </summary>
    public class NotAuditLogActionFilter : ActionFilterAttribute
    {
     
    }
}