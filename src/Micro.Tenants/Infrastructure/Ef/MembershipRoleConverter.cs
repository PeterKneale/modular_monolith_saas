using Micro.Tenants.Domain.Memberships;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Ef;

public class MembershipRoleConverter() : ValueConverter<MembershipRole, string>(v => v.Name, v => MembershipRole.FromString(v));