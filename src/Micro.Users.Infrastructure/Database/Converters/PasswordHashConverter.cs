using Micro.Users.Domain.Users;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Users.Infrastructure.Database.Converters;

public class PasswordHashConverter() : ValueConverter<PasswordHash, string>(v => v.Value, v => new PasswordHash(v));