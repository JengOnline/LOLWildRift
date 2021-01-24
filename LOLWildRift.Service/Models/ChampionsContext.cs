using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOLWildRift.Service.Models
{
    public class ChampionsContext : DbContext
    {
        public ChampionsContext(DbContextOptions<ChampionsContext> options) : base(options)
        {

        }
        public  DbSet<ChampionsEntity> Champions { get; set; }
        public  DbSet<ResultEntity> AddOrUpdate { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<RecommendedLaneEntity> Lanes { get; set; }


    }
}
