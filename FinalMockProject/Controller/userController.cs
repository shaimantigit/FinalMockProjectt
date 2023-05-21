using AutoMapper;
using FinalMockProject.BLL;
using FinalMockProject.DAL;
using FinalMockProject.DAL.Interface;
using FinalMockProject.Data;
using FinalMockProject.Models;
using FinalMockProject.Models.DTO;
using Microsoft.AspNetCore.Mvc;



namespace FinalMockProject.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly RentalService _rentalservice;
        private readonly UserRepository _userrepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private IMapper mapper;
     
        

        public UserController(ApplicationDbContext context,IMapper mapper, IUserRepository userRepository)
        {
            _context = context;
            _rentalservice = new RentalService(_context);
            _mapper = mapper;
            _userRepository = userRepository;

        }
        [HttpPost]
        public async  Task<IActionResult> Create([FromBody] AddUserDTO userDto)
        {
            var userEntity = _mapper.Map<User>(userDto);
            await _userRepository.CreateAsync(userEntity);
            var users = _mapper.Map<AddUserDTO>(userEntity);
           
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userEntity = await _userRepository.GetAllAsync();
            return Ok(_mapper.Map<List<AddUserDTO>>(userEntity));
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromBody]int id)
        {
            var userEntity = await _userRepository.GetById(id);
            if(userEntity == null)
            {
                return BadRequest();
            }
            var users = _mapper.Map<AddUserDTO>(userEntity);
            return Ok(users);
        }
        [HttpDelete("{User_Id}")]

        public IActionResult DeleteUser(int User_Id)
        {

            var users = _context.Products.Find(User_Id);
            if (users == null)
            {
                return NotFound();
            }
            _context.Products.Remove(users);
            _context.SaveChanges();
            return Ok("User deleted successfully");

        }



    }
}
