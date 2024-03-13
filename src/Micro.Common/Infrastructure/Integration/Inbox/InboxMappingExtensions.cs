using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Integration.Inbox;

public static class InboxMappingExtensions
{
    public static void AddInbox(this ModelBuilder modelBuilder, string schema)
    {
        modelBuilder.Entity<InboxMessage>(entity =>
        {
            entity.ToTable("inbox", schema);
            entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ProcessedAt).HasColumnName("processed_at");
        });
    }
}