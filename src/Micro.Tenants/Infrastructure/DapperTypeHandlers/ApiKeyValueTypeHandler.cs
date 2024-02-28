using System.Data;
using Dapper;
using Micro.Tenants.Domain.ApiKeys;

namespace Micro.Tenants.Infrastructure.DapperTypeHandlers;

public class ApiKeyValueTypeHandler : SqlMapper.TypeHandler<ApiKeyValue>
{
    private ApiKeyValueTypeHandler()
    {
    }

    public static readonly ApiKeyValueTypeHandler Default = new();

    public override ApiKeyValue Parse(object? value)
    {
        if (value is string name)
        {
            return new ApiKeyValue(name);
        }

        throw new FormatException($"Invalid conversion to {nameof(ApiKeyValue)}");
    }

    public override void SetValue(IDbDataParameter parameter, ApiKeyValue? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value?.Value;
    }
}