﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Micro.Tenants.Infrastructure.Database.Converters;

public class EmailAddressConverter() : ValueConverter<EmailAddress, string>(v => v.Value, v => EmailAddress.Create(v));