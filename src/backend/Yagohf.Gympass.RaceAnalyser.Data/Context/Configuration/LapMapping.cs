using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Yagohf.Gympass.RaceAnalyser.Model.Entities;

namespace Yagohf.Gympass.RaceAnalyser.Data.Context.Configuration
{
    public class LapMapping : IEntityTypeConfiguration<Lap>
    {
        public void Configure(EntityTypeBuilder<Lap> builder)
        {
            throw new NotImplementedException();
        }
    }
}
