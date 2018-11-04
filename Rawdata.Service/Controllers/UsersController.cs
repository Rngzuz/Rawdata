using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

        public UsersController(IMapper dtoMapper, IUserService service)
        {
            DtoMapper = dtoMapper;
            Service = service;
        }

        [Authorize()]
        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            UserDto user;

            try {
                var result = await Service.GetUserById(id);
                user = DtoMapper.Map<UserDto>(result);
            }
            catch {
                return StatusCode(500);
            }

            if (user == null) {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
