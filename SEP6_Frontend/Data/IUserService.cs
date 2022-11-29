using System.Threading.Tasks;
using SEP6_Frontend.Models;

namespace SEP6_Frontend.Data
{
    public interface IUserService {
        Task<User> ValidateLogin(string username, string password);
        Task<bool> Register(User user);
        Task<string> UpdateUser(User updatedUser);
    }
}