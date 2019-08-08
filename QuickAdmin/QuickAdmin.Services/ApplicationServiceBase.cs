using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuickAdmin.DAO;
using QuickAdmin.Model;
using QuickAdmin.Model.Entities;

namespace QuickAdmin.Services
{
    public class ApplicationServiceBase
    {
        protected readonly IConfiguration Configuration;
        protected readonly IHostingEnvironment HostingEnvironment;
        protected readonly IHttpContextAccessor HttpContextAccessor;
        protected readonly QuickAdminDbContext DbContext;

        public ApplicationServiceBase(
            IConfiguration configuration,
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor,
            QuickAdminDbContext dbContext)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            HttpContextAccessor = httpContextAccessor;
            DbContext = dbContext;
        }

        protected ApplicationUser CurrentUser =>
            new ApplicationUser()
            {
                Id = HttpContextAccessor.HttpContext.User.FindFirst("id")?.Value,
                UserName = HttpContextAccessor.HttpContext.User.FindFirst("name")?.Value,
                UserNo = HttpContextAccessor.HttpContext.User.FindFirst("userNo")?.Value
            };

        protected User CurrentUserAllInfo
        {
            get
            {
                var userId = HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return DbContext.User.FirstOrDefault(x => x.Id == userId);
            }
        }
    }
}
