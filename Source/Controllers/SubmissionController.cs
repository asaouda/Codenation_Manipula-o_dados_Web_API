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
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;
        private readonly IMapper _mapper;
        public SubmissionController(ISubmissionService service, IMapper mapper)
        {
            this._submissionService = service;
            this._mapper = mapper;
        }

        // GET api/submission
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<SubmissionDTO>> GetAll(int? accelerationId = null, int? challengeId = null)
        {
            if (accelerationId == null && challengeId == null)
            {
                return NoContent();
            }

            var _submissions = _submissionService.FindByChallengeIdAndAccelerationId(accelerationId ?? 0, challengeId ?? 0).ToList();
            
            if (_submissions == null)
            {
                return NoContent();
            }
            else
            {
                var retorno = _mapper.Map<List<SubmissionDTO>>(_submissions);
                return Ok(retorno);
            }
        }

        // GET api/submission/{higherScore}
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Decimal> GetHigherScore(int? challengeid)
        {
            if (challengeid == null)
            {
                return NoContent();
            }

            var higherScore = _submissionService.FindHigherScoreByChallengeId(challengeid ?? 0);
            return Ok(higherScore);
        }

        // POST api/company
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<SubmissionDTO> Post([FromBody] SubmissionDTO value)
        {
            var _value = _mapper.Map<Submission>(value);
            var _submission = _submissionService.Save(_value);

            if (_submission != null)
            {
                return Ok(_mapper.Map<SubmissionDTO>(_submission));
            }
            else
            {
                return NoContent();
            }
        }
    }
}
