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
        #region Property section
        private static IConfiguration _configuration;
        static HttpClient client = new HttpClient();
        /// <summary>
        /// http://localhost:50086/api/Champions/
        /// </summary>
        private readonly string url = "http://localhost:50099/api/Champions/";
        private static string apiKey = "";
        private static string username = "";
        private static string password = "";
        private readonly string configApiKey = "API-KEY";
        private readonly string configBasicAuth = "Authorization";
        #endregion

        #region public method 
        public void Dispose()
        {

        }
        public LOLWildRiftService()
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                apiKey = GetSectionValue(configApiKey);
                username = GetSectionValue("Username");
                password = GetSectionValue("Password");
            }
        }
        #region Champions
        /// <summary>
        /// Basic auth
        /// </summary>
        /// <returns></returns>
        public async Task<ChampionsList> ChampionList()
        {
            ChampionsList champions = new ChampionsList();
            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url + "ChampionsList");
                var auth = "Basic ";
                var encoding = Encoding.GetEncoding("iso-8859-1");
                auth += Convert.ToBase64String(encoding.GetBytes(username + ":" + password));
                requestMessage.Headers.Add(configBasicAuth, auth);
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();
                    champions = JsonConvert.DeserializeObject<ChampionsList>(responseStr);
                    champions.Error = false;
                }
                else
                {
                    champions.Error = true;
                }
            }
            catch (Exception)
            {
                champions = new ChampionsList();
                champions.Error = true;
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
                    else
                    {
                        champions.Error = true;
                    }
                }
            }
            catch (Exception)
            {
                champions = new ChampionsEntity();
                champions.Error = true;
            }
            return champions;
        }

        /// <summary>
        /// Auth API-KEY
        /// </summary>
        /// <param name="champion"></param>
        /// <returns></returns>
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

        #endregion

        #region Role

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

        public async Task<ResultEntity> RoleAddOrUpdate(RoleEntity role)
        {
            ResultEntity result = new ResultEntity();
            try
            {
                var reqBody = JsonConvert.SerializeObject(role);
                var httpContent = new StringContent(reqBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url + "RoleAddOrUpdate", httpContent);
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
            catch
            {
                result.RESULT = false;
            }
            return result;
        }

        public async Task<ResultEntity> RoleDelete(int id)
        {
            ResultEntity result = new ResultEntity();
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(url + "RoleDelete/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResultEntity>(responseStr);
                }
            }
            catch
            {
                result.RESULT = false;
            }
            return result;
        }

        #endregion
        #region RecommendedLane


        public async Task<RecommendedLaneList> RecommendedLaneList()
        {
            RecommendedLaneList lanes = new RecommendedLaneList();
            try
            {
                HttpResponseMessage response = await client.GetAsync(url+"RecommendedLaneList");
                if (response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();
                    lanes = JsonConvert.DeserializeObject<RecommendedLaneList>(responseStr);
                }
            }
            catch (Exception)
            {
                lanes = new RecommendedLaneList();
            }
            return lanes;
        }

        public async Task<ResultEntity> RecommendedLaneAddOrUpdate(RecommendedLaneEntity role)
        {
            ResultEntity result = new ResultEntity();
            try
            {
                var reqBody = JsonConvert.SerializeObject(role);
                var httpContent = new StringContent(reqBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(url + "RecommendedLaneAddOrUpdate", httpContent);
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
            catch
            {
                result.RESULT = false;
            }
            return result;
        }

        public async Task<ResultEntity> RecommendedLaneDelete(int id)
        {
            ResultEntity result = new ResultEntity();
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(url + "RecommendedLaneDelete/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ResultEntity>(responseStr);
                }
            }
            catch
            {
                result.RESULT = false;
            }
            return result;
        }

        #endregion

        #endregion

        #region Private method
        private static IConfiguration Configuration
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

        private static string GetSectionValue(string sectionName)
        {
            return Configuration.GetSection("Auth:" + sectionName).Value;
        }
        #endregion
    }
}
