﻿using Dapper;
using Micro.Common.Infrastructure.Database;
using static Micro.Tenants.Constants;

namespace Micro.Tenants.Application.Projects.Queries;

public static class ListProjects
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(Guid Id, string Name);

    private class Handler(IOrganisationExecutionContext executionContext, ConnectionFactory connections) : IRequestHandler<Query, IEnumerable<Result>>
    {
        public async Task<IEnumerable<Result>> Handle(Query query, CancellationToken token)
        {
            const string sql = $"SELECT {IdColumn},{NameColumn} " +
                               $"FROM {ProjectsTable} m " +
                               $"WHERE {OrganisationIdColumn} = @OrganisationId";
            using var con = connections.CreateConnection();
            return await con.QueryAsync<Result>(new CommandDefinition(sql, new
            {
                executionContext.OrganisationId
            }, cancellationToken: token));
        }
    }
}