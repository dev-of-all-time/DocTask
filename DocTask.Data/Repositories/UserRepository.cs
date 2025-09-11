using DocTask.Core.Interfaces.Repositories;
using DocTask.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DocTask.Data.Repositories;

public class UserRepository : IUserRepository
{
    private ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUserNameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> UpdateRefreshToken(User user, string refreshToken)
    {
        user.Refreshtoken = refreshToken;
        _context.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
}