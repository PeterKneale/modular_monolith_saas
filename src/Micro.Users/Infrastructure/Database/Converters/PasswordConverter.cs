using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Users.Infrastructure.Database.Converters;

public class PasswordConverter() : ValueConverter<Password, string>(v => v.Value, v => new Password(v));