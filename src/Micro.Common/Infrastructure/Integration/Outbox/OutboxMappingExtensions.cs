using Microsoft.EntityFrameworkCore;
using static Micro.Common.Infrastructure.Integration.Constants;

namespace Micro.Common.Infrastructure.Integration.Outbox;

public static class OutboxMappingExtensions
{
    public static void AddOutbox(this ModelBuilder modelBuilder, string schema)
    {
        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable(OutboxTable, schema);
            entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ProcessedAt).HasColumnName("processed_at");
        });
    }
}