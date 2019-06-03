using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Context.Configuration
{
    public class LapMapping : IEntityTypeConfiguration<Lap>
    {
        public void Configure(EntityTypeBuilder<Lap> builder)
        {
            builder.ToTable("Lap", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.RaceId)
             .HasColumnName("RaceId")
             .IsRequired();

            builder.Property(x => x.Date)
              .HasColumnName("Date")
              .IsRequired();

            builder.Property(x => x.DriverNumber)
              .HasColumnName("DriverNumber")
              .IsRequired();

            builder.Property(x => x.DriverName)
             .HasColumnName("DriverName")
             .IsRequired();

            builder.Property(x => x.Number)
             .HasColumnName("Number")
             .IsRequired();

            builder.Property(x => x.TimeTicks)
             .HasColumnName("TimeTicks")
             .IsRequired();

            builder.Ignore(x => x.Time);

            builder.Property(x => x.AverageSpeed)
             .HasColumnName("AverageSpeed")
             .IsRequired();
        }
    }
}
