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
    [Route("api/candidate")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;
        public CandidateController(ICandidateService service, IMapper mapper)
        {
            this._candidateService = service;
            this._mapper = mapper;
        }

        // GET api/company
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<CandidateDTO>> GetAll(int? accelerationId = null, int? companyId = null)
        {
            if ((accelerationId != null && companyId != null) || (accelerationId == null && companyId == null))
            {
                return NoContent();
            }

            IList<Candidate> _candidates;

            if (accelerationId != null)
            {
                _candidates = _candidateService.FindByAccelerationId(accelerationId ?? 0).ToList();
            }
            else
            {
                _candidates = _candidateService.FindByCompanyId(companyId ?? 0).ToList();
            }


            if (_candidates == null)
            {
                return NoContent();
            }
            else
            {
                var retorno = _mapper.Map<List<CandidateDTO>>(_candidates);
                return Ok(retorno);
            }
        }
        // GET api/Candidate/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CandidateDTO> Get(int accelerationId, int companyId, int userId)
        {
            var _user = _candidateService.FindById(userId,accelerationId,companyId);
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

        // POST api/company
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CandidateDTO> Post([FromBody] CandidateDTO value)
        {
            var _value = _mapper.Map<Candidate>(value);
            var _candidates = _candidateService.Save(_value);

            if (_candidates != null)
            {
                return Ok(_mapper.Map<CandidateDTO>(_candidates));
            }
            else
            {
                return NoContent();
            }
        }

    }
}

