﻿using System;
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
    public class UserController : ControllerBase
    {
        //直接注入QuickAdminDbContext或者IQuickAdminRepository<User>仓储，
        //或者直接注入IUserService，IUserService里面会注入仓储

        //private readonly QuickAdminDbContext _dbContext;
        private readonly IQuickAdminRepository<User> _userRepository;
        private readonly IUserService _userService;

        public UserController(IUserService userService,
            IQuickAdminRepository<User> userRepository,
            CommonHelper commonHelper)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser([FromQuery]GetUserInput input)
        {
            return new JsonResult(await _userService.GetUserById(input));
        }


        [HttpPost]
        [Route("AddUser")]
        [AllowAnonymous] //todo 正式开发时需要去掉
        public async Task<IActionResult> AddUser([FromBody]AddUserInput input)
        {
            return new JsonResult(await _userService.AddUser(input));
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userRepository.GetListAsync(x => true);
            return new JsonResult(await users);
        }
    }
}