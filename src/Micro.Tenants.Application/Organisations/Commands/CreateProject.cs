namespace Micro.Tenants.Application.Organisations.Commands;

public static class CreateProject
{
    public record Command(Guid ProjectId, string Name) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.ProjectId).NotEmpty();
            RuleFor(m => m.Name).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(IExecutionContext context, IOrganisationRepository organisations) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var organisationId = context.OrganisationId;
            var projectId = ProjectId.Create(command.ProjectId);
            var name = ProjectName.Create(command.Name);

            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), organisationId.Value);

            organisation.AddProject(projectId, name);
            organisations.Update(organisation);
        }
    }
}