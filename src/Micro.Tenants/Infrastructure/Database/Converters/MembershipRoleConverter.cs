using Micro.Tenants.Domain.OrganisationAggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Database.Converters;

public class MembershipRoleConverter() : ValueConverter<MembershipRole, string>(v => v.Name, v => MembershipRole.FromString(v));