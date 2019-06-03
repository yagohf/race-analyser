using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Context.Configuration
{
    public class RaceTypeMapping : IEntityTypeConfiguration<RaceType>
    {
        public void Configure(EntityTypeBuilder<RaceType> builder)
        {
            builder.ToTable("RaceType", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(x => x.Name)
             .HasColumnName("Name")
             .IsRequired();

            //Relacionamentos.
            builder.HasMany(x => x.Races)
                .WithOne(x => x.RaceType)
                .HasForeignKey(x => x.RaceTypeId);
        }
    }
}
