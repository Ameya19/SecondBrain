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
    internal class IngestionJobConfig : IEntityTypeConfiguration<IngestionJob>
    {
        public void Configure(EntityTypeBuilder<IngestionJob> builder)
        {
            builder.HasKey(ij => ij.Id);
            builder.Property(ij => ij.Status).HasMaxLength(50);
            builder.Property(ij => ij.ErrorMessage).HasMaxLength(1000);

            builder.HasOne(ij => ij.Source)
                   .WithMany(s => s.IngestionJobs)
                   .HasForeignKey(ij => ij.SourceId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
