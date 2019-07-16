using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAO;
using DAO.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;

namespace AspNetCoreQuickAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IQuickAdminRepository<User> _userRepository;
        private readonly QuickDbContext _context;

        public UserController(IQuickAdminRepository<User> userRepository,
            QuickDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        [HttpGet]
        [Route("getUser/{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = _userRepository.GetEntityAsync(x => x.Id == userId);
            return new JsonResult(await user);
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var user = _userRepository.GetListAsync(x => 2 > 1);
            return new JsonResult(await user);
        }

        [HttpPost]
        [Route("Test")]
        public IActionResult Test()
        {
            var userList = new List<User>()
            {
                new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "孙涛测试1",
                    EnUserName = "stwhh1"
                },
                new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "孙涛测试2",
                    EnUserName = "stwhh2"
                },
                new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "孙涛测试3",
                    EnUserName = "stwhh3"
                },
            };
            _context.User.AddRange(userList);
            var result = _context.SaveChanges();
            return new ContentResult() {Content = result > 0 ? "新增成功" : "新增失败"};
        }
    }
}