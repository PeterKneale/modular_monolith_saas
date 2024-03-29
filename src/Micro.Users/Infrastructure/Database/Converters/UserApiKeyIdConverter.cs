using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Users.Infrastructure.Database.Converters;

public class UserApiKeyIdConverter() : ValueConverter<UserApiKeyId, Guid>(v => v.Value, v => new UserApiKeyId(v));