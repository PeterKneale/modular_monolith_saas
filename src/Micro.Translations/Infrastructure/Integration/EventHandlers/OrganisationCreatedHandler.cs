using Micro.Tenants.IntegrationEvents;

namespace Micro.Translations.Infrastructure.Integration.EventHandlers;

public class OrganisationCreatedHandler(ILogger<OrganisationCreatedHandler> logs) : INotificationHandler<OrganisationCreated>
{
    public Task Handle(OrganisationCreated notification, CancellationToken cancellationToken)
    {
        logs.LogInformation($"Organisation created: {notification.OrganisationId}");
        return Task.CompletedTask;
    }
}