using DocTask.Core.Models;

namespace DocTask.Core.Interfaces.Repositories;

public interface IUserRepository
{ 
    Task<User?> GetByUserNameAsync(string username);
    Task<User> UpdateRefreshToken(User user, string refreshToken);
}