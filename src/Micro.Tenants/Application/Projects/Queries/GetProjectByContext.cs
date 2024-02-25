
using Micro.Common.Application;

namespace Micro.Tenants.Application.Projects.Queries;

public static class GetProjectByContext
{
    public record Query : IRequest<Result>;

    public record Result(Guid Id, string Name);

    public class Validator : AbstractValidator<Query>
    {
        
    }

    public class Handler(IProjectExecutionContext executionContext, IProjectRepository apps) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query query, CancellationToken token)
        {
            var app = await apps.GetAsync(executionContext.ProjectId, token);
            if (app == null)
            {
                throw new Exception("not found");
            }

            return new Result(app.Id.Value, app.Name.Value);
        }
    }
}