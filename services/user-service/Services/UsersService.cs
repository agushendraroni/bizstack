using UserService.Data;
using UserService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserService.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserDbContext _dbContext;  // <== Deklarasi field di sini

        public UsersService(UserDbContext dbContext)  // <== Inject di constructor
        {
            _dbContext = dbContext;  // <== Assign ke field
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();  // <-- pake _dbContext di sini
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }
    }
}
