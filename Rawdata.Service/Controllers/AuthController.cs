using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Rawdata.Data;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        protected readonly IMapper DtoMapper;
        protected readonly IUserService Service;

        public AuthController(IMapper dtoMapper, IUserService service)
        {
            DtoMapper = dtoMapper;
            Service = service;
        }

        [HttpPost("signin", Name = "RegisterUser")]
        [Produces("application/json")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            UserDto user;

            try {
                var newUser = DtoMapper.Map<User>(userRegisterDto);
                var result = await Service.RegisterUser(newUser);
                user = DtoMapper.Map<UserDto>(result);
            }
            catch (PostgresException exception) {
                if (exception.SqlState == "23505") {
                    return BadRequest();
                }

                return StatusCode(500);
            }
            catch {
                return StatusCode(500);
            }

            if (user == null) {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("oauth", Name = "SignIn")]
        [Produces("text/plain")]
        public async Task<IActionResult> SignIn(UserSignInDto userSignInDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var user = await Service.GetUserByEmail(userSignInDto.Email);

            if (user == null) {
                // Return 400 instead of 404, because they should not know user doesn't exist
                return BadRequest();
            }

            var isPasswordValid = PasswordHelper.Validate(userSignInDto.Password, user.Password);

            if (!isPasswordValid) {
                // Wrong credentials (same as wrong email or not found)
                return BadRequest();
            }

            string token;

            try {
                // Create token descriptor
                var descriptor = new SecurityTokenDescriptor {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim("id", user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.DisplayName),
                        new Claim(ClaimTypes.Email, user.Email)
                    }),
                    Expires = DateTime.Now.AddMinutes(10),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("L0onhppuCM1lMTwiEYe8667BZ-Bd8C22ETjdsdRm5NU")),
                        SecurityAlgorithms.HmacSha384Signature
                    )
                };

                var handler = new JwtSecurityTokenHandler();

                token = handler.WriteToken(
                    handler.CreateToken(descriptor)
                );
            }
            catch {
                return BadRequest();
            }

            return Ok(token);
        }
    }
}
