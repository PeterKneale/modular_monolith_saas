using Micro.Users.Domain.Users;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Users.Infrastructure.Infrastructure.Database.Converters;

public class HashedPasswordConverter() : ValueConverter<HashedPassword, string>(v => v.Value, v => new HashedPassword(v));