using System.Threading.Tasks;
using QuickAdmin.Model.DTO;

namespace QuickAdmin.Services.Interface
{
    public interface IUserService
    {
        Task<AddUserOutput> AddUserAsync(AddUserInput input);

        Task<GetUserOutput> GetUserById(GetUserInput input);
    }
}
