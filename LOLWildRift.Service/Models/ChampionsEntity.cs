﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LOLWildRift.Service.Models {
    public class ChampionsEntity {
        [JsonProperty ("ID")]
        public int ID { get; set; }

        [JsonProperty ("NAME")]
        public string NAME { get; set; }

        [JsonProperty ("HISTORY")]
        public string HISTORY { get; set; }

        [JsonProperty ("STATS_DAMAGE")]
        public int? STATS_DAMAGE { get; set; }

        [JsonProperty ("STATS_TOUGHNESS")]
        public int? STATS_TOUGHNESS { get; set; }

        [JsonProperty ("STATS_UTILITY")]
        public int? STATS_UTILITY { get; set; }

        [JsonProperty ("STATS_DIFFICULITY")]
        public int? STATS_DIFFICULITY { get; set; }

        [JsonProperty ("ROLE")]
        public string ROLE { get; set; }

        [JsonProperty ("LANE")]
        public string LANE { get; set; }

        [JsonProperty ("IMAGE_PATH")]
        public string IMAGE_PATH { get; set; }
    }

    public class ChampionsList {
        private List<ChampionsEntity> _champions;
        public ChampionsList () {
            _champions = new List<ChampionsEntity> ();
        }

        public List<ChampionsEntity> Champions {
            get { return _champions; }
            set { value = _champions; }
        }
    }

    public class ResultEntity {

        [JsonProperty ("RESULT")]
        [Key]
        public bool RESULT { get; set; }
    }

    public class ChampionAddEntity : ChampionsEntity {
        [JsonProperty ("ROLE_ID")]
        public int? ROLE_ID { get; set; }

        [JsonProperty ("RECOMMENDED_LANE_ID")]
        public int? RECOMMENDED_LANE_ID { get; set; }

    }

}