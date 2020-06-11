using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuickAdmin.Common;
using QuickAdmin.DAO;
using QuickAdmin.DAO.Repository;
using QuickAdmin.Model.DTO;
using QuickAdmin.Model.Entities;
using QuickAdmin.Services.Interface;

namespace QuickAdmin.Services.Imp
{
    public class UserService : ApplicationServiceBase, IUserService
    {
        private readonly IQuickAdminRepository<User> _userRepository;

        public UserService(IConfiguration configuration,
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor,
            QuickAdminDbContext dbContext,
            IQuickAdminRepository<User> userRepository)
            : base(configuration, hostingEnvironment, httpContextAccessor, dbContext)
        {
            _userRepository = userRepository;
        }

        public async Task<AddUserOutput> AddUserAsync(AddUserInput input)
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
