using LOLWildRift.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;


namespace LOLWildRift.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChampionsController : ControllerBase
    {
        private readonly IChampionsRepository _championsRepository;

        public ChampionsController(IChampionsRepository championsRepository)
        {
            _championsRepository = championsRepository;
        }

        [HttpPost]
        [Route("ChampionAddOrUpdate")]
        public async Task<IActionResult> ChampionAddOrUpdate([FromBody] ChampionAddEntity champion)
        {
            try
            {
                if (!string.IsNullOrEmpty(champion.NAME))
                {
                    return Ok(JsonConvert.SerializeObject(await _championsRepository.ChampionAddOrUpdate(champion)));
                }
                else return BadRequest("Name is required!!!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database" + ex.Message);
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
                    "Error retrieving data from the database");
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
                    "Error retrieving data from the database");
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
                    "Error retrieving data from the database");
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
                    "Error retrieving data from the database");
            }
        }
    }
}

