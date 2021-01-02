using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOLWildRift.Service.Models
{
    public class ChampionsRepository : IChampionsRepository
    {
        private readonly ChampionsContext _championsContext;

        public ChampionsRepository(ChampionsContext championsContext)
        {
            _championsContext = championsContext;
        }

        public async Task<Object> ChampionAddOrUpdate(ChampionAddEntity champion)
        {
            try
            {
                SqlParameter name = new SqlParameter("@name", champion.NAME ?? (object)DBNull.Value);
                SqlParameter history = new SqlParameter("@history", champion.HISTORY ?? (object)DBNull.Value);
                SqlParameter damage = new SqlParameter("@damage", champion.STATS_DAMAGE ?? (object)DBNull.Value);
                SqlParameter toughness = new SqlParameter("@toughness", champion.STATS_TOUGHNESS ?? (object)DBNull.Value);
                SqlParameter utility = new SqlParameter("@utility", champion.STATS_UTILITY ?? (object)DBNull.Value);
                SqlParameter difficulity = new SqlParameter("@difficulity", champion.STATS_DIFFICULITY ?? (object)DBNull.Value);
                SqlParameter roleId = new SqlParameter("@roleId", champion.ROLE_ID ?? (object)DBNull.Value);
                SqlParameter laneId = new SqlParameter("@laneId", champion.RECOMMENDED_LANE_ID ?? (object)DBNull.Value);
                SqlParameter imagePath = new SqlParameter("@imagePath", champion.IMAGE_PATH ?? (object)DBNull.Value);

                return await _championsContext.AddOrUpdate.FromSqlRaw(
                    $"EXEC [CHAMPIONS.SP_ADD_OR_UPDATE] @name, @history, @damage, @toughness," +
                    $"@utility, @difficulity, @roleId, @laneId, @imagePath",
                    name, history, damage, toughness, utility, difficulity, roleId, laneId, imagePath)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Object> GetChampion(int id)
        {
            ChampionsEntity champion = new ChampionsEntity();
            try
            {
                var response = await _championsContext.Champions.FromSqlInterpolated($"EXEC [CHAMPIONS.SP_GET] {id}").ToListAsync();
                if (response != null && response.Count > 0)
                {
                    champion = response[0];
                }
                return champion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Object> ChampionsList()
        {
            ChampionsList champions = new ChampionsList();
            try
            {
                var data = await _championsContext.Champions.FromSqlRaw($"EXEC [CHAMPIONS.SP_LIST]").ToListAsync();
                champions.Champions.AddRange(data);
                return champions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Object> RoleList()
        {
            RoleList roles = new RoleList();
            try
            {
                var data = await _championsContext.Roles.FromSqlRaw($"EXEC [ROLE.SP_LIST]").ToListAsync();
                roles.roles.AddRange(data);
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Object> RecommededLaneList()
        {
            RecommededLaneList lanes = new RecommededLaneList();
            try
            {
                var data = await _championsContext.Lanes.FromSqlRaw($"EXEC [RECOMMENDED_LANE.SP_LIST]").ToListAsync();
                lanes.Lanes.AddRange(data);
                return lanes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
