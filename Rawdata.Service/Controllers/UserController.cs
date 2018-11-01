using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Repositories.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected readonly IMapper DtoMapper;
        protected readonly IUserRepository Service;

        public UserController(IMapper mapper, IUserRepository service)
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

        [HttpGet("{id}/favorite_comments")]
        public async Task<IActionResult> GetFavoriteComments(int id)
        {
            var fComments = await Service.GetMarkedComments(id);

            if (!fComments.Any())
            {
                return NotFound(fComments);
            }

            return Ok(fComments);
        }
        //
//        [HttpGet("{id}/favorite_posts")]
//        public async Task<IActionResult> GetMarkedPosts(int id)
//        {
//            var fPosts = await Service.GetMarkedPosts(id);
//
//            if (!fPosts.Any())
//            {
//                return NotFound(fPosts);
//            }
//            var fPostDto = DtoMapper.Map<ICollection<FavoritePostDto>>(fPosts);
//
//            return Ok(fPostDto);
//        }
        //

        //        [HttpPost()]
        //        public IActionResult Delete(int userId)
        //        {
        //
        //        }

    }
}
