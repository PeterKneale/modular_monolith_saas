using System.Data;
using Dapper;
using Micro.Tenants.Domain.Memberships;

namespace Micro.Tenants.Infrastructure.DapperTypeHandlers;

public class MembershipIdTypeHandler : SqlMapper.TypeHandler<MembershipId>
{
    private MembershipIdTypeHandler()
    {
    }

    public static readonly MembershipIdTypeHandler Default = new();

    public override MembershipId Parse(object? value)
    {
        if (value is Guid id)
        {
            return new MembershipId(id);
        }

        throw new FormatException($"Invalid conversion to {nameof(MembershipId)}");
    }

    public override void SetValue(IDbDataParameter parameter, MembershipId? value)
    {
        parameter.DbType = DbType.Guid;
        parameter.Value = value?.Value;
    }
}