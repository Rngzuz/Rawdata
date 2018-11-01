using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        protected readonly IMapper DtoMapper;
        protected readonly IUserService Service;

        public UsersController(IMapper mapper, IUserService service)
        {
            DtoMapper = mapper;
            Service = service;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = Service.GetById(id).Result;

            if (user == null)
            {
                return NotFound(null);
            }
            return Ok(new UserDto(user));
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            // Url.Action(nameof(this.GetById), new { id = 1 });

            var user = Service.RegisterUser("test", registerDto.Email, "1234").Result;
            if (user == null)
            {
                return NotFound(user);
            }
            return Ok(new UserDto(user));
        }
    }
}
