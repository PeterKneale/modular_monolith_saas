using Micro.Users.Domain.Users;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Users.Infrastructure.Infrastructure.Database.Converters;

public class PasswordConverter() : ValueConverter<Password, string>(v => v.Value, v => Password.Create(v));