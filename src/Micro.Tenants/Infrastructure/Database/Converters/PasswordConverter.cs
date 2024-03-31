using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Database.Converters;

public class PasswordConverter() : ValueConverter<Password, string>(v => v.Value, v => Password.Create(v));