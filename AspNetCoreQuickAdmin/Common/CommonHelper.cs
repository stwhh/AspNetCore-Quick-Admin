using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Model;
using Model.Entities;

namespace Common
{
    /// <summary>
    /// 获取当前登录人信息
    /// </summary>
    public class CommonHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly QuickDbContext _dbContext;

        public CommonHelper(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            QuickDbContext dbContext
        )
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            _dbContext = dbContext;
        }

        public ApplicationUser CurrentUser =>
            new ApplicationUser()
            {
                Id = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                UserName = _httpContextAccessor.HttpContext.User.FindFirst("name")?.Value,
                UserNo = _httpContextAccessor.HttpContext.User.FindFirst("userNo")?.Value
            };

        public User CurrentUserAllInfo
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return _dbContext.User.FirstOrDefault(x => x.Id == userId);
            }
        }
    }
}
