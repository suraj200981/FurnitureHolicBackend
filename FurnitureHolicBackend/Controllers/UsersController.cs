using AuthenticationPlugin;
using FurnitureHolicBackend.Data;
using FurnitureHolicBackend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
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
        public IActionResult Register([FromForm] User user) {


            var userWithSameEmail = _dbContext.Users.Where(u => u.Email == user.Email).FirstOrDefault();

            if (userWithSameEmail != null)
            {
                return BadRequest("User with same email address already exists");
            }

            var uniqueNameForImage = Guid.NewGuid();
            var filePath = Path.Combine("wwwroot", uniqueNameForImage + ".jpg");

            if (user.Image != null) {
                var fileStream = new FileStream(filePath, FileMode.Create);
                user.Image.CopyTo(fileStream);
            }

            var regularUserReg = new User
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Password = SecurePasswordHasherHelper.Hash(user.Password),
                UserType = "Regular",
                ImageUrl = user.ImageUrl = filePath.Remove(0, 7)
        };

            _dbContext.Users.Add(regularUserReg);
            _dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        
        }

        [HttpPost]
        public IActionResult Login([FromBody] User user) {

            var emailCheck = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);

            if (emailCheck == null) {
                return NotFound("User email does not exist");
            }

            if (!SecurePasswordHasherHelper.Verify(user.Password, emailCheck.Password)) {
                return Unauthorized("Password is incorrect");
            }

            var claims = new[]
                {
                   new Claim(JwtRegisteredClaimNames.Email, user.Email),
                   new Claim(ClaimTypes.Email, user.Email),
                   new Claim(ClaimTypes.Role, emailCheck.UserType)
                };

            var token = _auth.GenerateAccessToken(claims);

            //return JWT Token
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_id = emailCheck.Id
            });

        }
        [Authorize]
        [HttpGet]
        public IActionResult AuthenticateUser() {

            return Ok("Tokken Authenticated");
        
        }

        [Authorize]
        [HttpGet]
        //api/movies/GetUserInformation?email=Suraj@hotmail.com  this would be the url for this method
        public IActionResult GetUserInformation(string email)
        {

            var userFound = from user in _dbContext.Users
                             where user.Email.StartsWith(email)
                             select new
                             {
                                 Name = user.Name,
                                 Phone = user.Phone,
                                 ImageUrl = user.ImageUrl
                             };

            if (userFound.All(a => string.IsNullOrEmpty(a.Name)))
            {
                return NotFound("User not found");
            }

            return Ok(userFound);
        }


    }
}
