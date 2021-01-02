using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOLWildRift.Service.Models
{
    public class RecommededLaneEntity
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("LANE")]
        public string LANE { get; set; }
    }

    public class RecommededLaneList
    {
        private List<RecommededLaneEntity> _lane;
        public RecommededLaneList()
        {
            _lane = new List<RecommededLaneEntity>();
        }

        public List<RecommededLaneEntity> Lanes
        {
            get { return _lane; }
            set { _lane = value; }
        }
    }
}
