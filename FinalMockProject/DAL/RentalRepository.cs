using FinalMockProject.DAL.Interface;
using FinalMockProject.Data;
using FinalMockProject.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FinalMockProject.DAL
{
    public class RentalRepository : IRentalRepository
    {
        private readonly ApplicationDbContext _context;
        public RentalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

     
        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }
        public async  Task<Product> GetProductById(int id) {
            return await _context.Products.FindAsync(id);
        }
        public async Task UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}

