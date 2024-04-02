﻿using Micro.Tenants.Domain.OrganisationAggregate;

namespace Micro.Tenants.Application.Organisations.Commands;

public static class UpdateProjectName
{
    public record Command(string Name) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Name).NotEmpty().MaximumLength(100);
        }
    }

    public class Handler(IOrganisationRepository organisations, IExecutionContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var organisationId = context.OrganisationId;
            var projectId = context.ProjectId;
            var projectName = ProjectName.Create(command.Name);

            var organisation = await organisations.GetAsync(organisationId, token);
            if (organisation == null) throw new NotFoundException(nameof(Organisation), organisationId.Value);

            organisation.UpdateProjectName(projectId, projectName);
        }
    }
}