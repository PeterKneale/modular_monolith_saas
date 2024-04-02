using Micro.Tenants.Domain.OrganisationAggregate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Database.Converters;

public class OrganisationNameConverter() : ValueConverter<OrganisationName, string>(v => v.Value, v => OrganisationName.Create(v));