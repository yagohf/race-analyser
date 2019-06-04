using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Context.Configuration
{
    public class RaceMapping : IEntityTypeConfiguration<Race>
    {
        public void Configure(EntityTypeBuilder<Race> builder)
        {
            builder.ToTable("Race", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Description)
             .HasColumnName("Description")
             .IsRequired();

            builder.Property(x => x.Date)
              .HasColumnName("Date")
              .IsRequired();

            builder.Property(x => x.RaceTypeId)
              .HasColumnName("RaceTypeId")
              .IsRequired();

            builder.Property(x => x.TotalLaps)
             .HasColumnName("TotalLaps")
             .IsRequired();

            builder.Property(x => x.UploaderId)
             .HasColumnName("UploaderId")
             .IsRequired();

            builder.Property(x => x.UploadDate)
             .HasColumnName("UploadDate")
             .IsRequired();

            //Relacionamentos.
            builder.HasMany(x => x.Laps)
                .WithOne(x => x.Race)
                .HasForeignKey(x => x.RaceId);

            builder.HasMany(x => x.DriverResults)
               .WithOne(x => x.Race)
               .HasForeignKey(x => x.RaceId);
        }
    }
}
