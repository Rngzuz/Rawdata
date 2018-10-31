using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rawdata.Data.Repositories.Interfaces;
using Rawdata.Service.Models;

namespace Rawdata.Service.Controllers
{
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
            var user = Service.GetById(id);

            if (user == null)
            {
                return NotFound();

            }
            var userDTO = DtoMapper.Map<UserDTO>(user);

            return Ok(userDTO);
        }

        [HttpGet("{id}")]
        public IActionResult GetFavoriteComments(int id)
        {
            // var fComments = Service.GetFavoriteComments(id);

            // if (fComments.Count <= 0)
            // {
            //     return NotFound(fComments);
            // }
            // var fCommentsDto = DtoMapper.Map<ICollection<FavoriteCommentDTO>>(fComments);

            // return Ok(fCommentsDto);
            throw new NotImplementedException();
        }

        public IActionResult GetFavoritePosts(int id)
        {
            // var fPosts = Service.GetFavoritePosts(id);
            // if (fPosts.Count <= 0)
            // {
            //     return NotFound(fPosts);
            // }
            // var fPostDto = DtoMapper.Map<ICollection<FavoritePostDTO>>(fPosts);

            // return Ok(fPostDto);
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public IActionResult Register(string name, string email, string password)
        {
            var user = Service.RegisterUser(name, email, password);
            if (user == null )
            {
                return NotFound(user);
            }
            var fPostDto = DtoMapper.Map<ICollection<UserDTO>>(user);

            return Ok(fPostDto);
        }

    }
}
