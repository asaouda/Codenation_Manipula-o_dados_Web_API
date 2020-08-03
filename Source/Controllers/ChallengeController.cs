using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Codenation.Challenge.DTOs;
using Codenation.Challenge.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Codenation.Challenge.Models;

namespace Codenation.Challenge.Controllers
{
    [Route("api/challenge")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        private readonly IChallengeService _challengeService;
        private readonly IMapper _mapper;
        public ChallengeController(IChallengeService service, IMapper mapper)
        {
            this._challengeService = service;
            this._mapper = mapper;
        }

        // GET api/challenge
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<ChallengeDTO>> GetAll(int? accelerationId = null, int? userId = null)
        {
            if (accelerationId == null && userId == null)
            {
                return NoContent();
            }
            

            var _challenge = _challengeService.FindByAccelerationIdAndUserId(accelerationId ?? 0, userId ?? 0).ToList();

            if (_challenge == null)
            {
                return NoContent();
            }
            else
            {
                var retorno = _mapper.Map<List<ChallengeDTO>>(_challenge);
                return Ok(retorno);
            }
        }


        // POST api/challenge
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ChallengeDTO> Post([FromBody] ChallengeDTO value)
        {
            var _value = _mapper.Map<Codenation.Challenge.Models.Challenge>(value);
            var _challenge = _challengeService.Save(_value);

            if (_challenge != null)
            {
                return Ok(_mapper.Map<ChallengeDTO>(_challenge));
            }
            else
            {
                return NoContent();
            }
        }

    }
}

