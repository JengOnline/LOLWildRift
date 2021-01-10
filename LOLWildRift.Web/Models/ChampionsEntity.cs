
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LOLWildRift.Web.Models
{
    public class ChampionsEntity : ErrorEntity
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("NAME")]
        public string NAME { get; set; }

        [JsonProperty("HISTORY")]
        public string HISTORY { get; set; }

        [JsonProperty("STATS_DAMAGE")]
        [Range(1, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]

        public int? STATS_DAMAGE { get; set; }

        [JsonProperty("STATS_TOUGHNESS")]
        [Range(1, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int? STATS_TOUGHNESS { get; set; }

        [JsonProperty("STATS_UTILITY")]
        [Range(1, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int? STATS_UTILITY { get; set; }

        [JsonProperty("STATS_DIFFICULITY")]
        [Range(1, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int? STATS_DIFFICULITY { get; set; }

        [JsonProperty("ROLE")]
        public string ROLE { get; set; }
        [JsonProperty("LANE")]
        public string LANE { get; set; }
        [JsonProperty("IMAGE_PATH")]
        public string IMAGE_PATH { get; set; }
    }

    public class ResultEntity 
    {

        [JsonProperty("RESULT")]
        [Key]
        public bool RESULT { get; set; }
    }

    public class ChampionsList : ErrorEntity
    {
        private List<ChampionsEntity> _champions;
        public ChampionsList()
        {
            _champions = new List<ChampionsEntity>();
        }

        public List<ChampionsEntity> Champions
        {
            get { return _champions; }
            set { value = _champions; }
        }


    }

    public class ChampionAddEntity : ChampionsEntity
    {
        [JsonProperty("ROLE_ID")]
        public int? ROLE_ID { get; set; }

        [JsonProperty("RECOMMENDED_LANE_ID")]
        public int? RECOMMENDED_LANE_ID { get; set; }

        [JsonProperty("IMAGE_FILE")]
        public IFormFile IMAGE_FILE { get; set; }
    }

    public class ErrorEntity
    {
        public bool Error { get; set; }

    }


}
