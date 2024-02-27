using System.Data;
using Dapper;
using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Infrastructure.DapperTypeHandlers;

public class ApiKeyNameTypeHandler : SqlMapper.TypeHandler<ApiKeyName>
{
    private ApiKeyNameTypeHandler()
    {
    }

    public static readonly ApiKeyNameTypeHandler Default = new();

    public override ApiKeyName Parse(object? value)
    {
        if (value is string name)
        {
            return new ApiKeyName(name);
        }

        throw new FormatException($"Invalid conversion to {nameof(ApiKeyName)}");
    }

    public override void SetValue(IDbDataParameter parameter, ApiKeyName? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value?.Name;
    }
}