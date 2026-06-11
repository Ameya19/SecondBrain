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
    public class ChunkTagConfig : IEntityTypeConfiguration<ChunkTag>
    {
        public void Configure(EntityTypeBuilder<ChunkTag> builder)
        {
            builder.HasKey(x => new { x.ChunkId, x.TagId });

            builder.HasOne(x => x.Chunk)
                .WithMany(x => x.ChunkTags)
                .HasForeignKey(x => x.ChunkId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Tag)
                .WithMany(x => x.ChunkTags)
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
