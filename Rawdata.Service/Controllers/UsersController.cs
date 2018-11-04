using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Rawdata.Data.Models;
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

        [HttpPost(Name = "RegisterUser")]
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
    }
}
