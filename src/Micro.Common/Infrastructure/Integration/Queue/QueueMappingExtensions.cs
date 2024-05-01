using Microsoft.EntityFrameworkCore;
using static Micro.Common.Infrastructure.Integration.Constants;

namespace Micro.Common.Infrastructure.Integration.Queue;

public static class QueueMappingExtensions
{
    public static void AddQueue(this ModelBuilder modelBuilder, string schema)
    {
        modelBuilder.Entity<QueueMessage>(entity =>
        {
            entity.ToTable(QueueTable, schema);
            entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ProcessedAt).HasColumnName("processed_at");
        });
    }
}