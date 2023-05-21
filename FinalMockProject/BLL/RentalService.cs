using FinalMockProject.DAL;
using FinalMockProject.DAL.Interface;
using FinalMockProject.Data;
using FinalMockProject.Models;
using FinalMockProject.Models.DTO;

namespace FinalMockProject.BLL
{
    public class RentalService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRentalRepository _rentalrepository;
        private readonly IUserRepository _userrepository;



        public RentalService(ApplicationDbContext context)
        {
            _context = context;
            _rentalrepository = new RentalRepository(_context);
            _userrepository = new UserRepository(_context);

        }






        public List<Product> GetAllProducts()
        {
            return _rentalrepository.GetAllProducts();
        }
        public async Task<Product> GetProductById(int id)
        {
            return await _rentalrepository.GetProductById(id);
        }
        public async Task UpdateProduct(Product product)
        {
            await _rentalrepository.UpdateProduct(product);
        }
        public User GetAllUsersById(int id)
        {
            return _context.Users.FirstOrDefault(U => U.User_Id == id);
        }
        
    }

}

