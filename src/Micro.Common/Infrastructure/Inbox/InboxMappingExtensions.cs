using Microsoft.EntityFrameworkCore;

namespace Micro.Common.Infrastructure.Inbox;

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
        });
    }
}