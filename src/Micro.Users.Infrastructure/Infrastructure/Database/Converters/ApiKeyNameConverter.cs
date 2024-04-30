using Micro.Users.Domain.ApiKeys;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Users.Infrastructure.Infrastructure.Database.Converters;

public class ApiKeyNameConverter() : ValueConverter<ApiKeyName, string>(v => v.Value, v => new ApiKeyName(v));