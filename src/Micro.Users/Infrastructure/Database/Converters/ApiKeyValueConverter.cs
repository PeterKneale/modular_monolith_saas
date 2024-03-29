using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Users.Infrastructure.Database.Converters;

public class ApiKeyValueConverter() : ValueConverter<ApiKeyValue, string>(v => v.Value, v => new ApiKeyValue(v));