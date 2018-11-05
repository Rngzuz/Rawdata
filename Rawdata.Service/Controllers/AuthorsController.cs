using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Services.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
    [ApiController]
    [Route("api/authors")]
    [Produces("application/json")]
    public class AuthorsController : ControllerBase
    {
        protected readonly IMapper DtoMapper;
        protected readonly IAuthorService AuthorService;

        public AuthorsController(IMapper dtoMapper, IAuthorService authorService)
        {
            DtoMapper = dtoMapper;
            AuthorService = authorService;
        }

        [HttpGet("{id:int}", Name = "GetAuthorById")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var result = await AuthorService.GetAuthorById(id);

            if (result == null) {
                return NotFound();
            }

            return Ok(
                DtoMapper.Map<AuthorDto>(result)
            );
        }
    }
}
