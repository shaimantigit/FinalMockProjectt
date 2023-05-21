//using FinalMockProject.Migrations;
using FinalMockProject.Models;

namespace FinalMockProject.DAL.Interface
{
    public interface IRentalRepository
    {
        
        List<Product> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task UpdateProduct(Product product);

    }


}
