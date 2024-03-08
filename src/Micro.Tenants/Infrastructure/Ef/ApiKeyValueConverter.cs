using Micro.Tenants.Domain.ApiKeys;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Ef;

public class ApiKeyValueConverter() : ValueConverter<ApiKeyValue, string>(v => v.Value, v => new ApiKeyValue(v));