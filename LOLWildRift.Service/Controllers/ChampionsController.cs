using LOLWildRift.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Security.Principal;
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
        private static string apiKey = "";
        private static string username = "";
        private static string password = "";


        public ChampionsController(IChampionsRepository championsRepository, IConfiguration configuration)
        {
            _championsRepository = championsRepository;
            _configuration = configuration;
            if (string.IsNullOrEmpty(apiKey))
            {
                apiKey = _configuration["Auth:API-KEY"];
                username = _configuration["Auth:Username"];
                password = _configuration["Auth:Password"];
            }
        }

        /// <summary>
        /// Auth API-Key
        /// </summary>
        /// <param name="champion"></param>
        /// <returns></returns>
        [Route("ChampionAddOrUpdate")]
        [HttpPost]
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

        [Route("GetChampion")]
        [HttpGet]
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

        /// <summary>
        ///  Basic Auth
        /// </summary>
        /// <returns></returns>
        [Route("ChampionsList")]
        [HttpGet]
        public async Task<IActionResult> ChampionsList()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authHeader))
                {
                    if (AuthenticateUser(authHeader))
                    {
                        return Ok(JsonConvert.SerializeObject(await _championsRepository.ChampionsList()));
                    }
                    else
                    {
                        return BadRequest("authentication is not match.");
                    }
                }
                else
                {
                    return BadRequest("authentication is null or emtry");
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Service temporary and not available");
            }
        }

        [Route("ChampionDelete")]
        [HttpPost]
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

        [Route("RoleList")]
        [HttpGet]
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

        [Route("RoleAddOrUpdate")]
        [HttpPut]
        public async Task<IActionResult> RoleAddOrUpdate(RoleEntity role)
        {
            try
            {
                if (role.ID > 0)
                {
                    return Ok(JsonConvert.SerializeObject(await _championsRepository.RoleAddOrUpdate(role.ID, role.ROLE_NAME)));
                }
                else return BadRequest("ID is required!!!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Service temporary and not available" + ex.Message);
            }
        }

        [Route("RoleDelete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> RoleDelete(int id)
        {
            try
            {
                if (id > 0)
                {
                    return Ok(JsonConvert.SerializeObject(await _championsRepository.RoleDelete(id)));
                }
                else return BadRequest("ID is required!!!");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Service temporary and not available" + ex.Message);
            }
        }

        [Route("RecommededLaneList")]
        [HttpGet]
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



        /// <summary>
        /// Basic Auth
        /// </summary>
        /// <param name="authHeader"></param>
        /// <returns></returns>
        private bool AuthenticateUser(string authHeader)
        {
            bool authPass = false;
            try
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);
                if (authHeaderVal.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && authHeaderVal.Parameter != null)
                {
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    var credentials = encoding.GetString(Convert.FromBase64String(authHeaderVal.Parameter));
                    int separator = credentials.IndexOf(':');
                    string name = credentials.Substring(0, separator);
                    string pass = credentials.Substring(separator + 1);

                    if (username == name && password == pass)
                    {
                        authPass = true;
                    }
                    else
                    {
                        authPass = false;
                    }
                }
                else
                {
                    authPass = false;
                }
            }
            catch (FormatException)
            {
                authPass = false;
            }
            return authPass;
        }




    }
}

