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
    [Route("api/camps/{moniker}/{talkId}/[controller]")]
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

        [HttpGet("/api/[controller]/{id:int}")]
        public async Task<ActionResult<SpeakerModel>> Get(int id)
        {
            try
            {
                var speaker = await _repository.GetSpeakerAsync(id);

                if (speaker == null) return NotFound("Speaker was not found");

                return _mapper.Map<SpeakerModel>(speaker);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [Route("/api/camps/{moniker}/[controller]")]
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
        
        
        [HttpPost]
        public async Task<ActionResult<SpeakerModel>> Post(string moniker, int talkId, SpeakerModel model)
        {
            try
            {
                var camp = await _repository.GetCampAsync(moniker);
                if (camp == null) return BadRequest("Camp does not exist");

                var talk = await _repository.GetTalkByMonikerAsync(moniker, talkId);
                if (talk == null) return BadRequest("Talk does not exist");

                var speaker = _mapper.Map<Speaker>(model);
                speaker.Talk = talk;
                speaker.Talk.Camp = camp;

                _repository.Add(speaker);

                if (await _repository.SaveChangesAsync())
                {
                    var location = _linkGenerator.GetPathByAction("Get",
                    "Speakers",
                    new { moniker, talkId, id = model.SpeakerId });

                    if (string.IsNullOrEmpty(location))
                    {
                        return BadRequest("Could not use current speaker ID");
                    }

                    return Created(location, _mapper.Map<SpeakerModel>(speaker));
                }

                var speakers = await _repository.GetAllSpeakersAsync();

                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest("Failed to save new speaker");
        }
    }
}
