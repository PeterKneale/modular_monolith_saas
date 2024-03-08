using Micro.Tenants.Domain.ApiKeys;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Ef;

public class ApiKeyNameConverter() : ValueConverter<ApiKeyName, string>(v => v.Name, v => new ApiKeyName(v));