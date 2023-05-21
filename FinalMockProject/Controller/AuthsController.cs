using AutoMapper;
using FinalMockProject.BLL;
using FinalMockProject.DAL.Interface;
using FinalMockProject.Data;
using FinalMockProject.Helper;
using FinalMockProject.Models;
using FinalMockProject.Models.Authentication;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalMockProject.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userrepository;

        public AuthsController(ApplicationDbContext context, IConfiguration configuration, IMapper mapper, IUserRepository userrepository)
        {
            _context = context;
            _mapper = mapper;
            _userrepository = userrepository;

            _configuration = configuration;


            //_rentalservice = new RentalService(_context);

        }

     
        [Route("login")]
        [HttpPost]
        public IActionResult Login(AuthUserLogin loginModel)
        {

            User user = _context.Users.Include(x => x.Roles).SingleOrDefault(x => x.User_Email == loginModel.User_email);

            if (user is null)
                return Unauthorized("Invalid Username or Password!");

            string hashedPassword = HashPassword(loginModel.User_Password);
            if (BCrypt.Net.BCrypt.Verify(loginModel.User_Password, user.User_Password))
            {

                var token = JWT.GenerateToken(new Dictionary<string, string> {
                { ClaimTypes.Role, user.Roles.RoleName  },
                { "RoleId", user.Roles.RoleId.ToString() },
                {JwtClaimTypes.PreferredUserName, user.User_Name },
                { JwtClaimTypes.Id, user.User_Id.ToString() },
                { JwtClaimTypes.Email, user.User_Email}
            }, _configuration["JWT:Key"]);



                return Ok(new AuthResponse { token = token, User_Name = user.User_Name });
            }
            else
            {
                return Unauthorized("Invalid Username or Password");
            }
        }
        [Route("Registration")]
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] AuthAddUserDTO addUserDTO)
        {

            // Check if a user with the same email already exists
            var existingUser = await _userrepository.GetByEmailAsync(addUserDTO.User_Email);
            if (existingUser != null)
            {
                // Return an error response indicating that the email is already registered
                return BadRequest("Email is already registered.");
            }
            //Map DTO to Domain Model           
            var userEntity = _mapper.Map<User>(addUserDTO);
            userEntity.User_Password = HashPassword(addUserDTO.User_Password);



            await _userrepository.CreateAsync(userEntity);
            // var users = mapper.Map<UserDTO>(userEntity);

            return Ok("Registration Successful");
        }
        private string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
            return hashedPassword;
        }
    }
}

