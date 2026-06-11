using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecondBrain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondBrain.Infrastructure.Config
{
    public class ContradictionConfig : IEntityTypeConfiguration<Contradiction>
    {
        public void Configure(EntityTypeBuilder<Contradiction> builder) 
        {
            builder.HasKey(c => c.Id);

            // Two FKs to the same table — EF Core needs explicit relationship names
            builder.HasOne(x => x.ChunkA)
                .WithMany(x => x.ContradictionsA)
                .HasForeignKey(x => x.ChunkAId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ChunkB)
                .WithMany(x => x.ContradictionsB)
                .HasForeignKey(x => x.ChunkBId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
