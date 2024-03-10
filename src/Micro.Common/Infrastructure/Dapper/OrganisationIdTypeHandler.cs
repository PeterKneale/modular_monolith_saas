using System.Data;
using Dapper;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Dapper;

public class OrganisationIdTypeHandler : SqlMapper.TypeHandler<OrganisationId>
{
    public static readonly OrganisationIdTypeHandler Default = new();

    private OrganisationIdTypeHandler()
    {
    }

    public override OrganisationId Parse(object? value)
    {
        if (value is Guid id) return new OrganisationId(id);

        throw new FormatException($"Invalid conversion to {nameof(OrganisationId)}");
    }

    public override void SetValue(IDbDataParameter parameter, OrganisationId? value)
    {
        parameter.DbType = DbType.Guid;
        parameter.Value = value?.Value;
    }
}