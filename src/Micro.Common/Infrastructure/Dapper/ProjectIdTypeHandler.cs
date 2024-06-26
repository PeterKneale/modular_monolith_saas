﻿using System.Data;
using Dapper;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Dapper;

public class ProjectIdTypeHandler : SqlMapper.TypeHandler<ProjectId>
{
    public static readonly ProjectIdTypeHandler Default = new();

    private ProjectIdTypeHandler()
    {
    }

    public override ProjectId Parse(object? value)
    {
        if (value is Guid id) return ProjectId.Create(id);

        throw new FormatException($"Invalid conversion to {nameof(ProjectId)}");
    }

    public override void SetValue(IDbDataParameter parameter, ProjectId? value)
    {
        parameter.DbType = DbType.Guid;
        parameter.Value = value?.Value;
    }
}