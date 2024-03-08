using Micro.Tenants.Domain.ApiKeys;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Database.Converters;

public class ApiKeyValueConverter() : ValueConverter<ApiKeyValue, string>(v => v.Value, v => new ApiKeyValue(v));