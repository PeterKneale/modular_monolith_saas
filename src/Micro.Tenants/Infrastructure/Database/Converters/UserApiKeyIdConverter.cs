using Micro.Tenants.Domain.ApiKeys;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Database.Converters;

public class UserApiKeyIdConverter() : ValueConverter<UserApiKeyId, Guid>(v => v.Value, v => new UserApiKeyId(v));