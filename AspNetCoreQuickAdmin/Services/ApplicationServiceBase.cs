using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DAO;
using DAO.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Model;
using Model.Entities;

namespace Services
{
    public class ApplicationServiceBase
    {
        protected readonly IConfiguration Configuration;
        protected readonly IHostingEnvironment HostingEnvironment;
        protected readonly IHttpContextAccessor HttpContextAccessor;
        protected readonly QuickDbContext DbContext;

        public ApplicationServiceBase(
            IConfiguration configuration,
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor,
            QuickDbContext dbContext)
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
