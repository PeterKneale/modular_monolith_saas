using Micro.Common.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Common.Infrastructure.Database.Converters;

public class OrganisationIdConverter()
    : ValueConverter<OrganisationId, Guid>(v => v.Value, v => OrganisationId.Create(v));