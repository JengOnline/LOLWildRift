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

        public async Task<Object> ChampionDelete(int id)
        {
            try
            {
                return await _championsContext.AddOrUpdate.FromSqlInterpolated($"EXEC [CHAMPIONS.SP_DELETE] {id}").ToListAsync();
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
                var data = await _championsContext.Roles.FromSqlRaw($"EXEC [ROLES.SP_LIST]").ToListAsync();
                roles.roles.AddRange(data);
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Object> RoleAddOrUpdate(int id, string role)
        {
            ResultEntity result = new ResultEntity();
            try
            {
                var response = await _championsContext.AddOrUpdate.FromSqlInterpolated($"EXEC [ROLES.SP_ADD_OR_UPDATE] {id},{role}").ToListAsync();
                if (response != null && response.Count > 0)
                {
                    result = response[0];
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Object> RoleDelete(int id)
        {
            ResultEntity result = new ResultEntity();
            try
            {
                var response = await _championsContext.AddOrUpdate.FromSqlInterpolated($"EXEC [ROLES.SP_DELETE] {id}").ToListAsync();
                if (response != null && response.Count > 0)
                {
                    result = response[0];
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Object> RecommendedLaneList()
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

        public async Task<Object> RecommendedLaneAddOrUpdate(int id, string lane)
        {
            ResultEntity result = new ResultEntity();
            try
            {
                var response = await _championsContext.AddOrUpdate.FromSqlInterpolated($"EXEC [RECOMMENDED_LANE.SP_ADD_OR_UPDATE] {id},{lane}").ToListAsync();
                if (response != null && response.Count > 0)
                {
                    result = response[0];
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Object> RecommendedLaneDelete(int id)
        {
            ResultEntity result = new ResultEntity();
            try
            {
                var response = await _championsContext.AddOrUpdate.FromSqlInterpolated($"EXEC [RECOMMENDED_LANE.SP_DELETE] {id}").ToListAsync();
                if (response != null && response.Count > 0)
                {
                    result = response[0];
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
