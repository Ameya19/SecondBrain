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
    public class QueryChunkLinkConfig : IEntityTypeConfiguration<QueryChunkLink>
    {
        public void Configure(EntityTypeBuilder<QueryChunkLink> builder)
        {
            builder.HasKey(x => new { x.QueryId, x.ChunkId });

            builder.HasOne(x => x.Query)
                .WithMany(q => q.QueryChunkLinks)
                .HasForeignKey(x => x.QueryId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);

            builder.HasOne(x => x.Chunk)
                .WithMany(cl => cl.QueryChunkLinks)
                .HasForeignKey(x => x.ChunkId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
        }
    }
}
