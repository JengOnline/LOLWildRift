using LOLWildRift.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;


namespace LOLWildRift.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChampionsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IChampionsRepository _championsRepository;
        private string apiKey = "";

        public ChampionsController(IChampionsRepository championsRepository, IConfiguration configuration)
        {
            _championsRepository = championsRepository;
            _configuration = configuration;
            if (string.IsNullOrEmpty(apiKey))
            {
                apiKey = _configuration["API-KEY"];
            }
        }

        [HttpPost]
        [Route("ChampionAddOrUpdate")]
        public async Task<IActionResult> ChampionAddOrUpdate([FromBody] ChampionAddEntity champion)
        {
            try
            {
                if (Request.Headers["API-KEY"] == apiKey)
                {
                    if (!string.IsNullOrEmpty(champion.NAME))
                    {
                        return Ok(JsonConvert.SerializeObject(await _championsRepository.ChampionAddOrUpdate(champion)));
                    }
                    else return BadRequest("Name is required!!!");
                }
                else
                {
                    return BadRequest("API-KEY not match");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Service temporary and not available" + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetChampion")]
        public async Task<IActionResult> GetChampion([FromHeader] int id)
        {
            try
            {
                if (id > 0)
                {
                    return Ok(JsonConvert.SerializeObject(await _championsRepository.GetChampion(id)));
                }
                else return BadRequest("id is required!!!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Service temporary and not available");
            }
        }

        [HttpGet]
        [Route("ChampionsList")]
        public async Task<IActionResult> ChampionsList()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _championsRepository.ChampionsList()));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Service temporary and not available");
            }
        }

        [HttpGet]
        [Route("RoleList")]
        public async Task<IActionResult> RoleList()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _championsRepository.RoleList()));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Service temporary and not available");
            }
        }

        [HttpGet]
        [Route("RecommededLaneList")]
        public async Task<IActionResult> RecommededLaneList()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(await _championsRepository.RecommededLaneList()));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Service temporary and not available");
            }
        }

        [HttpPost]
        [Route("ChampionDelete")]
        public async Task<IActionResult> ChampionDelete([FromHeader] int id)
        {
            try
            {
                if (id > 0)
                {
                    return Ok(JsonConvert.SerializeObject(await _championsRepository.ChampionDelete(id)));
                }
                else return BadRequest("ID is required!!!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Service temporary and not available" + ex.Message);
            }
        }
    }
}

