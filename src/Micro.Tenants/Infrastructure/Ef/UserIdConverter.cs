using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Ef;

public class UserIdConverter() : ValueConverter<UserId, Guid>(v => v.Value, v => new UserId(v));