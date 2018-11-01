using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Repositories.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Produces("applicaiton/json")]
    public class UsersController : ControllerBase
    {
        protected readonly IMapper DtoMapper;
        protected readonly IUserRepository Service;

        public UsersController(IMapper mapper, IUserRepository service)
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
        public IActionResult Register([FromBody] string email)
        {
            var user = Service.RegisterUser("test", email, "1234").Result;
            if (user == null)
            {
                return NotFound(user);
            }
            return Ok(new UserDto(user));
        }
    }
}
