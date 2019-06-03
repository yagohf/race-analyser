using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Context.Configuration
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Login)
             .HasColumnName("Login")
             .IsRequired();

            builder.Property(x => x.Password)
              .HasColumnName("Password")
              .IsRequired();

            builder.Property(x => x.Name)
              .HasColumnName("Name")
              .IsRequired();

            //Relacionamentos.
            builder.HasMany(x => x.Races)
                .WithOne(x => x.Uploader)
                .HasForeignKey(x => x.UploaderId);
        }
    }
}
