using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOLWildRift.Web.Models
{
    public class RoleEntity
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("ROLE_NAME")]
        public string ROLE_NAME { get; set; }

    }

    public class RoleList
    {
        private List<RoleEntity> _role;

        public RoleList()
        {
            _role = new List<RoleEntity>();
        }

        public List<RoleEntity> roles
        {
            get { return _role; }
            set { _role = value; }
        }
    }
}
