using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [ApiController]
    [Route("api")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        protected readonly IMapper DtoMapper;

        public AuthController(IMapper dtoMapper)
        {
            DtoMapper = dtoMapper;
        }

        // public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        // {
        //     var newUser = DtoMapper.Map<User>(userRegisterDto);
        // }

        // [HttpPost("", Name = "Auth_SignIn")]
        // public IActionResult SignIn([FromBody] string username, [FromBody] string password)
        // {

        // }
    }
}
