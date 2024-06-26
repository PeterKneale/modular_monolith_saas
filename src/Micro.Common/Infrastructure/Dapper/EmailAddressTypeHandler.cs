﻿using System.Data;
using Dapper;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Dapper;

public class EmailAddressTypeHandler : SqlMapper.TypeHandler<EmailAddress>
{
    public static readonly EmailAddressTypeHandler Default = new();

    private EmailAddressTypeHandler()
    {
    }

    public override EmailAddress Parse(object? value)
    {
        if (value is string email) return EmailAddress.Create(email);

        throw new FormatException($"Invalid conversion to {nameof(EmailAddress)}");
    }

    public override void SetValue(IDbDataParameter parameter, EmailAddress? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value?.Display;
    }
}