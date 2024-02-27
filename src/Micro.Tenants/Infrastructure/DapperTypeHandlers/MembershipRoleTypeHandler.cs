using System.Data;
using Dapper;
using Micro.Tenants.Domain.Memberships;

namespace Micro.Tenants.Infrastructure.DapperTypeHandlers;

public class MembershipRoleTypeHandler : SqlMapper.TypeHandler<MembershipRole>
{
    private MembershipRoleTypeHandler()
    {
    }

    public static readonly MembershipRoleTypeHandler Default = new();

    public override MembershipRole Parse(object? value)
    {
        if (value is string name)
        {
            return MembershipRole.FromString(name);
        }

        throw new FormatException($"Invalid conversion to {nameof(MembershipRole)}");
    }

    public override void SetValue(IDbDataParameter parameter, MembershipRole? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value?.Name;
    }
}