using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.Core.Domain.Games;
using VerticalSliceArchitecture.Core.Domain.Identity;
using VerticalSliceArchitecture.Infrastructure.Data.EntitiesConfiguration;

namespace VerticalSliceArchitecture.Infrastructure.Data
{
    public class AppDb : DbContext
    {
        public AppDb()
        { }

       
        public AppDb(DbContextOptions<AppDb> options) : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            UserConfiguration.Setup(builder);
            SessionConfiguration.Setup(builder);
            GameConfiguration.Setup(builder);

        }
       
    }
}