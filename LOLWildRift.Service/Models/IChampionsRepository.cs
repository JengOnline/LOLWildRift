using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOLWildRift.Service.Models
{
    public interface IChampionsRepository
    {
        public Task<Object> ChampionAddOrUpdate(ChampionAddEntity champion);
        public Task<Object> GetChampion(int id);
        public Task<Object> ChampionsList();
        public Task<Object> RoleList();
        public Task<Object> RoleAddOrUpdate(int id ,string role);
        public Task<Object> RoleDelete(int id);
        public Task<Object> RecommededLaneList();
        public Task<Object> ChampionDelete(int id);
    }
}
