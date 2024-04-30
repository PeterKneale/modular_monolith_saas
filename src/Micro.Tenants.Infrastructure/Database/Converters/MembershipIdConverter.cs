namespace Micro.Tenants.Infrastructure.Database.Converters;

public class MembershipIdConverter() : ValueConverter<MembershipId, Guid>(v => v.Value, v => new MembershipId(v));