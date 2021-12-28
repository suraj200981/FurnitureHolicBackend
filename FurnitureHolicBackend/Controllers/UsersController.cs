using AuthenticationPlugin;
using FurnitureHolicBackend.Data;
using FurnitureHolicBackend.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureHolicBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private FurnitureDbContext _dbContext;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public UsersController(FurnitureDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpPost]
        public IActionResult Register([FromBody] User user) {


            var userWithSameEmail = _dbContext.Users.Where(u => u.Email == user.Email).FirstOrDefault();

            if (userWithSameEmail != null)
            {
                return BadRequest("User with same email address already exists");
            }

            var regularUserReg = new User
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Password = SecurePasswordHasherHelper.Hash(user.Password),
                UserType = "Regular"
            };

            _dbContext.Users.Add(regularUserReg);
            _dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        
        }

    }
}
