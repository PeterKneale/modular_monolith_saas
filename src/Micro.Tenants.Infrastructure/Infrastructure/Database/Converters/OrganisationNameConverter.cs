namespace Micro.Tenants.Infrastructure.Infrastructure.Database.Converters;

public class OrganisationNameConverter() : ValueConverter<OrganisationName, string>(v => v.Value, v => OrganisationName.Create(v));