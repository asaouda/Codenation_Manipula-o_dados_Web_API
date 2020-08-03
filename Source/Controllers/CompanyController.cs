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
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        public CompanyController(ICompanyService service, IMapper mapper)
        {
            this._companyService = service;
            this._mapper = mapper;
        }

        // GET api/company
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<CompanyDTO>> GetAll(int? accelerationId = null, int? userId = null)
        {
            if ((accelerationId != null && userId != null) || (accelerationId == null && userId == null))
            {
                return NoContent();
            }

            IList<Company> _companies;

            if (accelerationId != null)
            {
                _companies = _companyService.FindByAccelerationId (accelerationId ?? 0).ToList();
            }
            else
            {
                _companies = _companyService.FindByUserId(userId ?? 0).ToList();
            }


            if (_companies == null)
            {
                return NoContent();
            }
            else
            {
                var retorno = _mapper.Map<List<CompanyDTO>>(_companies);
                return Ok(retorno);
            }
        }

        // GET api/company/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CompanyDTO> Get(int id)
        {
            var _company = _companyService.FindById(id);
            if (_company != null)
            {
                var retorno = _mapper.Map<CompanyDTO>(_company);
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
        public ActionResult<CompanyDTO> Post([FromBody] CompanyDTO value)
        {
            var _value = _mapper.Map<Company>(value);
            var _company = _companyService.Save(_value);

            if (_company != null)
            {
                return Ok(_mapper.Map<CompanyDTO>(_company));
            }
            else
            {
                return NoContent();
            }
        }

    }
}

