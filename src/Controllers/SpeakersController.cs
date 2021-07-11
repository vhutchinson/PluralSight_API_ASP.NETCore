using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers
{
    [ApiController]
    [Route("api/camps/{moniker}/[controller]")]
    public class SpeakersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICampRepository _repository;
        private readonly LinkGenerator _linkGenerator;

        public SpeakersController(IMapper mapper, ICampRepository repository, LinkGenerator linkGenerator)
        {
            _mapper = mapper;
            _repository = repository;
            _linkGenerator = linkGenerator;
        }

        // Get all Speakers.
        [Route("/api/[controller]")]
        [HttpGet]
        public async Task<ActionResult<SpeakerModel[]>> Get()
        {
            try
            {
                var speakers = await _repository.GetAllSpeakersAsync();

                return _mapper.Map<SpeakerModel[]>(speakers);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        // Get Speaker by ID.
        [HttpGet("/api/[controller]/{id:int}")]
        public async Task<ActionResult<SpeakerModel>> Get(int id)
        {
            try
            {
                var speaker = await _repository.GetSpeakerAsync(id);
                if (speaker == null) return NotFound($"Speaker with id {id} was not found");

                return _mapper.Map<SpeakerModel>(speaker);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        // Get Speaker by Camp moniker.
        [HttpGet]
        public async Task<ActionResult<SpeakerModel[]>> GetByCamp(string moniker)
        {
            try
            {
                var speakers = await _repository.GetSpeakersByMonikerAsync(moniker);

                return _mapper.Map<SpeakerModel[]>(speakers);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        // Get Speakers corresponding to a company.
        [HttpGet("search")]
        public async Task<ActionResult<SpeakerModel[]>> SearchByCompany(string company)
        {
            try
            {
                var speakers = await _repository.GetAllSpeakersByCompany(company);

                if (!speakers.Any()) return NotFound($"No speakers found for company {company}");

                return _mapper.Map<SpeakerModel[]>(speakers);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

    }
}
