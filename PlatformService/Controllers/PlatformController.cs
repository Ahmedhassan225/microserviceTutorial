using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformController(IPlatformRepo platformRepo, IMapper mapper, ICommandDataClient commandDataClient)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Platform>> GetPlatforms()
        {
            Console.WriteLine("--> Getting All Platforms");

            var platforms = _platformRepo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatfromReadDto>>(platforms));
        }

        [HttpGet("{Id}", Name = "GetPlatformById")]
        public ActionResult<Platform> GetPlatformById(int Id)
        {
            Console.WriteLine($"--> Getting Platform with id ={Id}");

            var platform = _platformRepo.GetPlatformById(Id);

            if (platform != null)
            { 
                return Ok(_mapper.Map<PlatfromReadDto>(platform));
            }

            return NotFound();
        }

        [HttpPost()]
        public async Task<ActionResult<PlatfromReadDto>> CreatePlatform(PlatformCreateDto platformCreate)
        {
            Console.WriteLine("--> Creating a Platform");
            
            var platformEntity = _mapper.Map<Platform>(platformCreate);

            _platformRepo.CreatePlatform(platformEntity);
            _platformRepo.SaveChanges();

            var platformRead = _mapper.Map<PlatfromReadDto>(platformEntity);
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformRead);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not Send synchronesly - {ex}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformRead.Id}, platformRead);
        }
    }
}
