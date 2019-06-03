using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Context.Configuration
{
    public class RaceTypeMapping : IEntityTypeConfiguration<RaceType>
    {
        public void Configure(EntityTypeBuilder<RaceType> builder)
        {
            throw new NotImplementedException();
        }
    }
}
