using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickAdmin.Common;
using QuickAdmin.DAO.Repository;
using QuickAdmin.Model.DTO;
using QuickAdmin.Model.Entities;
using QuickAdmin.Services.Interface;

namespace QuickAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserManageController : ControllerBase
    {
        //直接注入QuickAdminDbContext或者IQuickAdminRepository<User>仓储，
        //或者直接注入IUserService，IUserService里面会注入仓储

        //private readonly QuickAdminDbContext _dbContext;
        private readonly IQuickAdminRepository<User> _userRepository;
        private readonly IUserService _userService;

        public UserManageController(IUserService userService,
            IQuickAdminRepository<User> userRepository,
            CommonHelper commonHelper)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("User/{id}")]
        public async Task<IActionResult> GetUserAsync(string id) //[FromQuery] GetUserInput input
        {
            return new JsonResult(await _userRepository.GetEntityAsync(x => x.Id == id));
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = _userRepository.GetListAsync(x => true);
            return new JsonResult(await users);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input">新增的用户信息参数</param>
        /// <returns></returns>
        [HttpPost]
        [Route("User")]
        [AllowAnonymous] //todo-stwhh 正式开发时需要去掉
        public async Task<IActionResult> AddUserAsync([FromBody] AddUserInput input)
        {
            return new JsonResult(await _userService.AddUserAsync(input));
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("User")]
        public async Task<IActionResult> UpdateUserAysnc(AddUserInput input)
        {
            var user = await _userRepository.GetEntityAsync(x => x.Id == input.Id);
            user.UserName = input.UserName;
            user.Phone = input.Phone;
            return new JsonResult(await _userRepository.UpdateAsync(user));
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("User/{id}")]
        public async Task<IActionResult> DeleteUserAysnc(string id)
        {
            var isSuccess = _userRepository.DeleteAsync(x => x.Id == id);
            return new JsonResult(await isSuccess);
        }
    }
}