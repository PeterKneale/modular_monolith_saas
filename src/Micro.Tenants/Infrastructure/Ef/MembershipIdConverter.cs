using Micro.Tenants.Domain.Memberships;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Ef;

public class MembershipIdConverter() : ValueConverter<MembershipId, Guid>(v => v.Value, v => new MembershipId(v));