using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecondBrain.Core.Entities;

namespace SecondBrain.Infrastructure.Config
{
    public class KnowledgeChunkConfig : IEntityTypeConfiguration<KnowledgeChunk>
    {
        public void Configure(EntityTypeBuilder<KnowledgeChunk> builder)
        {
            builder.HasKey(kc => kc.Id);
            builder.Property(x => x.Embedding).HasColumnType("vector(768)");

            builder.HasIndex(x => x.Embedding)
                .HasMethod("hnsw")
                .HasOperators("vector_cosine_ops");

            builder.HasOne(x => x.Source)
                .WithMany(s => s.KnowledgeChunks)
                .HasForeignKey(x => x.SourceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
