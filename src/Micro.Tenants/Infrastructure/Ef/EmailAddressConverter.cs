using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Ef;

public class EmailAddressConverter() : ValueConverter<EmailAddress, string>(v => v.Value, v => new EmailAddress(v));