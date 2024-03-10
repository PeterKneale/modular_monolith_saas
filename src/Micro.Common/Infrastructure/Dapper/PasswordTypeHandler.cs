using System.Data;
using Dapper;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Dapper;

public class PasswordTypeHandler : SqlMapper.TypeHandler<Password>
{
    public static readonly PasswordTypeHandler Default = new();

    private PasswordTypeHandler()
    {
    }

    public override Password Parse(object? value)
    {
        if (value is string password) return new Password(password);

        throw new FormatException($"Invalid conversion to {nameof(EmailAddress)}");
    }

    public override void SetValue(IDbDataParameter parameter, Password? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value?.Value;
    }
}