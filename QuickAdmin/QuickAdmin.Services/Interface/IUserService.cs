using Model.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IUserService
    {
        Task<AddUserOutput> AddUser(AddUserInput input);

        Task<GetUserOutput> GetUserById(GetUserInput input);
    }
}
