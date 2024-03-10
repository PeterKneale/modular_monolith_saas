using Micro.Tenants.Domain.ApiKeys;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Database.Converters;

public class ApiKeyNameConverter() : ValueConverter<ApiKeyName, string>(v => v.Value, v => new ApiKeyName(v));