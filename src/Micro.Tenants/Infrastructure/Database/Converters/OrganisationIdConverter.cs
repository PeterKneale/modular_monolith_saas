using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Database.Converters;

public class OrganisationIdConverter() : ValueConverter<OrganisationId, Guid>(v => v.Value, v => new OrganisationId(v));