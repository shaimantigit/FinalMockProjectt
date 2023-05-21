using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FinalMockProject.Models.Authentication.AuthUserLogin;
using System.Security.Claims;
using AutoMapper;
using FinalMockProject.BLL;
using FinalMockProject.DAL.Interface;
using FinalMockProject.Data;
using IdentityModel;
using FinalMockProject.Models;
using FinalMockProject.Helper;
using FinalMockProject.Models.Authentication;
using FinalMockProject.DAL;
using FinalMockProject.Models.DTO;

namespace FinalMockProject.Controller
{
    public class AuthController :ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly RentalService _rentalservice;

        private readonly IConfiguration _configuration;
        private IMapper _mapper;
        private readonly IUserRepository _userrepository;

        public AuthController(ApplicationDbContext context, IConfiguration configuration, IMapper mapper, IUserRepository userrepository)
        {
            _context = context;
            _mapper = mapper;
            _userrepository = userrepository;

            this._configuration = configuration;


            _rentalservice = new RentalService(_context);

        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(AuthUserLogin loginModel)
        {
           

            var user = _context.Users.Include(x => x.Roles).SingleOrDefault(x => x.User_Email == loginModel.User_email);

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

        public async Task<IActionResult> Create([FromBody] AuthAddUserDTO authAddUserDto)
        {
            //Map DTO to Domain Model           
            var userEntity = _mapper.Map<User>(authAddUserDto);
            userEntity.User_Password = HashPassword(authAddUserDto.User_Password);
            await _userrepository.CreateAsync(userEntity);
            var users = _mapper.Map<AuthAddUserDTO>(userEntity);

            return Ok(users);
        }
        private string HashPassword(string Password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
            return hashedPassword;
        }
    }
}





  