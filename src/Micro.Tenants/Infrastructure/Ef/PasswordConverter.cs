using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Ef;

public class PasswordConverter() : ValueConverter<Password, string>(v => v.Value, v => new Password(v));