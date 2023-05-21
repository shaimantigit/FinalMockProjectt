using AutoMapper;
using FinalMockProject.BLL;
using FinalMockProject.DAL;
using FinalMockProject.DAL.Interface;
using FinalMockProject.Data;
using FinalMockProject.Models;
using FinalMockProject.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace FinalMockProject.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly RentalService _rentalservice;
      
        private IMapper _mapper;

        public RentalController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;


            _rentalservice = new RentalService(_context);
        }
        
        
        [HttpGet("products")]


        public IActionResult GetAllProducts()
        {

            var products = _rentalservice.GetAllProducts();
            return Ok(products);

        }

        [HttpPost]

        public ActionResult<Product> AddProduct([FromBody] ProductDTO productDto)
        {
            if (productDto == null)
            {
                return BadRequest(productDto);
            }


            var products = _mapper.Map<Product>(productDto);
            _context.Products.Add(products);
            _context.SaveChanges();
            return Ok(products);

        }
        [HttpDelete("{Product_Id}")]

        public IActionResult DeleteProduct(int Product_Id)
        {

            var products = _context.Products.Find(Product_Id);
            if (products == null)
            {
                return NotFound();
            }
            _context.Products.Remove(products);
            _context.SaveChanges();
            return Ok("Product deleted successfully");

        }
        [HttpPut("{id:int}", Name = "UpdateProduct")]

        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDTO productDto)
        {

            var products = await _rentalservice.GetProductById(id);

            if (products == null || id != products.Product_Id)
            {
                return BadRequest();
            }
            products.Product_Name = productDto.Product_Name;
            products.Description = productDto.Description;
            products.Security_Deposit = productDto.Security_Deposit;
            products.Rental_Price = productDto.Rental_Price;
            await _rentalservice.UpdateProduct(products);
            return NoContent();

        }


        
    }


    }
   




     

