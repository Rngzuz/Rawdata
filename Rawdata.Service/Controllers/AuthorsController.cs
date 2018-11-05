using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Services.Interfaces;

namespace Rawdata.Service.Controllers
{
    [ApiController]
    [Route("api/questions")]
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
            throw new NotImplementedException();
        }
    }
}
