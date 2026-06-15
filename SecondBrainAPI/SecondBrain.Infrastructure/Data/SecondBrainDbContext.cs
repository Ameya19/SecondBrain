using Microsoft.EntityFrameworkCore;
using SecondBrain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace SecondBrain.Infrastructure.Data
{
    public class SecondBrainDbContext : DbContext
    {
        public SecondBrainDbContext(DbContextOptions<SecondBrainDbContext> options) : base(options)
        {
        }

        public DbSet<KnowledgeChunk> KnowledgeChunks => Set<KnowledgeChunk>();
        public DbSet<Source> Sources => Set<Source>();
        public DbSet<IngestionJob> IngestionJobs => Set<IngestionJob>();
        public DbSet<ChunkTag> ChunkTags => Set<ChunkTag>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Query> Queries => Set<Query>();
        public DbSet<QueryChunkLink> QueryChunkLinks => Set<QueryChunkLink>();
        public DbSet<Contradiction> Contradictions => Set<Contradiction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SecondBrainDbContext).Assembly);

            modelBuilder.HasPostgresExtension("vector");
        }
    }
}
