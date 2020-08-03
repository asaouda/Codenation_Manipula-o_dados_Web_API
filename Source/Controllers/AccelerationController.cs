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
    [Route("api/acceleration")]
    [ApiController]
    public class AccelerationController : ControllerBase
    {
        private readonly IAccelerationService _accelerationService;
        private readonly IMapper _mapper;
        public AccelerationController(IAccelerationService service, IMapper mapper)
        {
            this._accelerationService = service;
            this._mapper = mapper;
        }

        // GET api/acceleration
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<AccelerationDTO>> GetAll(int? companyId = null)
        {
            if (companyId == null)
            {
                return NoContent();
            }

            var _accelerations = _accelerationService.FindByCompanyId(companyId ?? 0).ToList();

            if (_accelerations == null)
            {
                return NoContent();
            }
            else
            {
                var retorno = _mapper.Map<List<AccelerationDTO>>(_accelerations);
                return Ok(retorno);
            }
        }

        // GET api/acceleration/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AccelerationDTO> Get(int id)
        {
            var _acceleration = _accelerationService.FindById(id);
            if (_acceleration != null)
            {
                var retorno = _mapper.Map<AccelerationDTO>(_acceleration);
                return Ok(retorno);
            }
            else
            {
                return NoContent();
            }
        }

        // POST api/acceleration
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AccelerationDTO> Post([FromBody] AccelerationDTO value)
        {
            var _value = _mapper.Map<Acceleration>(value);
            var _acceleration = _accelerationService.Save(_value);

            if (_acceleration != null)
            {
                return Ok(_mapper.Map<AccelerationDTO>(_acceleration));
            }
            else
            {
                return NoContent();
            }
        }
    }
}
