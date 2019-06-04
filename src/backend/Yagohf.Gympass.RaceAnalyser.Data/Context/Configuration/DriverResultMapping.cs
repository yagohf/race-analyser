using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Context.Configuration
{
    public class DriverResultMapping : IEntityTypeConfiguration<DriverResult>
    {
        public void Configure(EntityTypeBuilder<DriverResult> builder)
        {
            builder.ToTable("DriverResult", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.RaceId)
                .HasColumnName("RaceId")
                .IsRequired();

            builder.Property(x => x.Position)
                .HasColumnName("Position")
                .IsRequired();

            builder.Property(x => x.DriverNumber)
                .HasColumnName("DriverNumber")
                .IsRequired();

            builder.Property(x => x.DriverName)
                .HasColumnName("DriverName")
                .IsRequired();

            builder.Property(x => x.Laps)
                .HasColumnName("Laps")
                .IsRequired();

            builder.Property(x => x.TotalRaceTimeTicks)
                .HasColumnName("TotalRaceTimeTicks")
                .IsRequired();

            builder.Ignore(x => x.TotalRaceTime);

            builder.Property(x => x.BestLapTicks)
                .HasColumnName("BestLapTicks")
                .IsRequired();

            builder.Ignore(x => x.BestLap);

            builder.Property(x => x.AverageSpeed)
                .HasColumnName("AverageSpeed")
                .IsRequired();

            builder.Property(x => x.GapTicks)
                .HasColumnName("GapTicks")
                .IsRequired();

            builder.Ignore(x => x.Gap);
        }
    }
}
