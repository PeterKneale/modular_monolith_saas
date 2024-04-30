using Micro.Users.Domain.ApiKeys;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Users.Infrastructure.Infrastructure.Database.Converters;

public class UserApiKeyIdConverter() : ValueConverter<UserApiKeyId, Guid>(v => v.Value, v => UserApiKeyId.Create(v));