using Micro.Tenants.Domain.Organisations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Ef;

public class OrganisationNameConverter() : ValueConverter<OrganisationName, string>(v => v.Value, v => OrganisationName.Create(v));