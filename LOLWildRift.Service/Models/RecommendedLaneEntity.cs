using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOLWildRift.Service.Models
{
    public class RecommendedLaneEntity
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("LANE")]
        public string LANE { get; set; }
    }

    public class RecommendedLaneList
    {
        private List<RecommendedLaneEntity> _lane;
        public RecommendedLaneList()
        {
            _lane = new List<RecommendedLaneEntity>();
        }

        public List<RecommendedLaneEntity> Lanes
        {
            get { return _lane; }
            set { _lane = value; }
        }
    }
}
