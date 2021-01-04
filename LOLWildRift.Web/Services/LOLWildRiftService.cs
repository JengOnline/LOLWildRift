using LOLWildRift.Web.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LOLWildRift.Web.Services
{
    public class LOLWildRiftService : IDisposable
    {
        private static IConfiguration _configuration;
        static HttpClient client = new HttpClient();
        private readonly string url = "http://localhost:50086/api/Champions/";
        private readonly string apiKey = "";
        private readonly string configApiKey = "API-KEY";

        public void Dispose()
        {

        }
        public LOLWildRiftService()
        {
            apiKey = GetSectionValue(configApiKey);
        }

        public async Task<ChampionsList> ChampionList()
        {
            ChampionsList champions = new ChampionsList();
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + "ChampionsList");
                if (response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();
                    champions = JsonConvert.DeserializeObject<ChampionsList>(responseStr);
                }
            }
            catch (Exception)
            {
                champions = new ChampionsList();
            }
            return champions;
        }

        public async Task<ChampionsEntity> GetChampion(int? id)
        {
            ChampionsEntity champions = new ChampionsEntity();
            try
            {
                if (id.HasValue)
                {
                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url + "GetChampion");
                    requestMessage.Headers.Add("id", id.ToString());
                    HttpResponseMessage response = await client.SendAsync(requestMessage);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStr = await response.Content.ReadAsStringAsync();
                        champions = JsonConvert.DeserializeObject<ChampionsEntity>(responseStr);
                    }
                }
            }
            catch (Exception)
            {
                champions = new ChampionsEntity();
            }
            return champions;
        }

        public async Task<RoleList> RoleList()
        {
            RoleList roles = new RoleList();
            try
            {
                HttpResponseMessage response = await client.GetAsync(url + "RoleList");
                if (response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();
                    roles = JsonConvert.DeserializeObject<RoleList>(responseStr);
                }
            }
            catch (Exception)
            {
                roles = new RoleList();
            }
            return roles;
        }

        public async Task<RecommededLaneList> RecommededLaneList()
        {
            RecommededLaneList lanes = new RecommededLaneList();
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:50086/api/Champions/RecommededLaneList");
                if (response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();
                    lanes = JsonConvert.DeserializeObject<RecommededLaneList>(responseStr);
                }
            }
            catch (Exception)
            {
                lanes = new RecommededLaneList();
            }
            return lanes;
        }

        public async Task<ResultEntity> ChampionAddOrUpdate(ChampionAddEntity champion)
        {
            ResultEntity result = new ResultEntity();
            try
            {
                string reqBody = JsonConvert.SerializeObject(champion);
                HttpRequestMessage reqMessage = new HttpRequestMessage(HttpMethod.Post, url + "ChampionAddOrUpdate");
                reqMessage.Headers.Add(configApiKey, apiKey);
                reqMessage.Content = new StringContent(reqBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.SendAsync(reqMessage);
                if (response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResultEntity>(responseStr);
                }
                else
                {
                    result.RESULT = false;
                }
            }
            catch (Exception)
            {
                result.RESULT = false;
            }
            return result;

        }

        public async Task<ResultEntity> ChampionDelete(int? id)
        {
            ResultEntity result = new ResultEntity();
            try
            {
                if (id.HasValue)
                {
                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url + "ChampionDelete");
                    requestMessage.Headers.Add("id", id.ToString());
                    HttpResponseMessage response = await client.SendAsync(requestMessage);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStr = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<ResultEntity>(responseStr);
                    }
                }
            }
            catch (Exception)
            {
                result.RESULT = false;
            }
            return result;
        }


       
        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(
                        Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange:
                        false);
                    _configuration = builder.Build();
                }

                return _configuration;
            }
            set
            {
                _configuration = value;
            }
        }

        public static string GetSectionValue(string sectionName)
        {
            return Configuration.GetSection(sectionName).Value;
        }

    }
}
