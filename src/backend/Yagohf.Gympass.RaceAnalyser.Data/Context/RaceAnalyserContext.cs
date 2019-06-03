using Microsoft.EntityFrameworkCore;
using Yagohf.Gympass.RaceAnalyser.Data.Context.Configuration;

namespace Yagohf.Gympass.RaceAnalyser.Data.Context
{
    public class RaceAnalyserContext : DbContext
    {
        public RaceAnalyserContext(DbContextOptions<RaceAnalyserContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new LapMapping());
            modelBuilder.ApplyConfiguration(new RaceTypeMapping());
            modelBuilder.ApplyConfiguration(new RaceMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
