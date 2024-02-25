using System.Data;
using Dapper;
using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Infrastructure.Dapper;

public class UserApiKeyIdTypeHandler : SqlMapper.TypeHandler<UserApiKeyId>
{
    private UserApiKeyIdTypeHandler()
    {
    }

    public static readonly UserApiKeyIdTypeHandler Default = new();

    public override UserApiKeyId Parse(object? value)
    {
        if (value is Guid id)
        {
            return new UserApiKeyId(id);
        }

        throw new FormatException($"Invalid conversion to {nameof(UserApiKeyId)}");
    }

    public override void SetValue(IDbDataParameter parameter, UserApiKeyId? value)
    {
        parameter.DbType = DbType.Guid;
        parameter.Value = value?.Value;
    }
}