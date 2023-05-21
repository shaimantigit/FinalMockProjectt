using FinalMockProject.DAL.Interface;
using FinalMockProject.Data;
using FinalMockProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalMockProject.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            
            _context = context;
        }
        
        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x=>x.User_Id == id);
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.Include("UserRole").FirstOrDefaultAsync(x => x.User_Email == email);
        }
    }
}
