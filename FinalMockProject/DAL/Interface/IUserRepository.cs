using FinalMockProject.Models;

namespace FinalMockProject.DAL.Interface
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<List<User>> GetAllAsync();
        Task<User> GetById(int id);
        Task<User> GetByEmailAsync(string email);

    }
}
