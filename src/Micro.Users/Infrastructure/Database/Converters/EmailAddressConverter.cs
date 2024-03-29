using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Users.Infrastructure.Database.Converters;

public class EmailAddressConverter() : ValueConverter<EmailAddress, string>(v => v.Value, v => new EmailAddress(v));