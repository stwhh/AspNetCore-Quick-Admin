using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DAO;
using DAO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model.DTO;
using Model.Entities;
using Services.Interface;

namespace Services.Imp
{
    public class UserService : ApplicationServiceBase, IUserService
    {
        private readonly IQuickAdminRepository<User> _userRepository;
        public UserService(IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor,
            QuickDbContext dbContext,
            IQuickAdminRepository<User> userRepository)
            : base(configuration, httpContextAccessor, dbContext)
        {
            _userRepository = userRepository;
        }

        public async Task<AddUserOutput> AddUser(AddUserInput input)
        {
            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = input.UserName,
                Password = EncryptHelper.AesEncrypt(Configuration["EncryptionKey"], input.Password),
                Phone = input.Phone
            };
            await DbContext.User.AddAsync(user);
            var result = await DbContext.SaveChangesAsync();
            return new AddUserOutput()
            {
                IsSuccess = result > 0,
                Msg = result > 0 ? "新增成功" : "新增失败"
            };
        }

        public async Task<GetUserOutput> GetUserById(GetUserInput input)
        {
            //var user = await _dbContext.User.Where(x => x.Id == input.UserId).FirstOrDefaultAsync();
            var user = await _userRepository.GetEntityAsync(x => x.Id == input.UserId);
            if (user == null)
            {
                return null;
            }
            return new GetUserOutput()
            {
                Id = user.Id,
                UserName = user.UserName,
                EnUserName = user.EnUserName,
                Password = user.Password,
                Email = user.Email,
                Phone = user.Phone
            };
        }


       
    }
}
