using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Codenation.Challenge.DTOs;
using Codenation.Challenge.Models;
using Codenation.Challenge.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codenation.Challenge.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService service, IMapper mapper)
        {
            this._userService = service;
            this._mapper = mapper;
        }

        // GET api/user
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<UserDTO>> GetAll(string accelerationName = null, int? companyId = null)
        {
            if((accelerationName!=null && companyId!=null) || (accelerationName == null && companyId == null))
            {
                return NoContent();
            }

            IList<User> _users;

            if (accelerationName != null)
            {
                _users = _userService.FindByAccelerationName(accelerationName).ToList();
            }
            else 
            {
                _users = _userService.FindByCompanyId(companyId ?? 0).ToList();
            }

           
            if (_users == null)
            {
                return NoContent();
            }
            else
            {
                var retorno = _mapper.Map<List<UserDTO>>(_users);
                return Ok(retorno);
            }
        }

        // GET api/user/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDTO> Get(int id)
        {
            var _user = _userService.FindById(id);
            if (_user != null)
            {
                var retorno = _mapper.Map<UserDTO>(_user);
                return Ok(retorno);
            }
            else
            {
                return NoContent();
            }
        }

        // POST api/user
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDTO> Post([FromBody] UserDTO value)
        {
            var _value = _mapper.Map<User>(value);
            var _user = _userService.Save(_value);

            if (_user != null)
            {
                return Ok(_mapper.Map<UserDTO>(_user));
            }
            else
            {
                return NoContent();
            }
        }   
     
    }
}
