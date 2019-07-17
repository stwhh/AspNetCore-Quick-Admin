using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DAO;
using DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Model;
using Model.Entities;

namespace Services
{
    public class ApplicationServiceBase
    {
        protected readonly IConfiguration Configuration;
        protected readonly IHttpContextAccessor HttpContextAccessor;
        protected readonly QuickDbContext DbContext;

        public ApplicationServiceBase(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            QuickDbContext dbContext
        )
        {
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
            DbContext = dbContext;
        }

        protected ApplicationUser CurrentUser =>
            new ApplicationUser()
            {
                Id = HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
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
