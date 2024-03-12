using Micro.Translations.Infrastructure.Database;
using Micro.Translations.IntegrationEvents;

namespace Micro.Translations.Infrastructure.Integration;

public class ProcessOutboxCommandHandler(Db db, IPublisher publisher, ILogger<ProcessOutboxCommandHandler> log) : IRequestHandler<ProcessOutboxCommand>
{
    public async Task<Unit> Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        var messages = await db.Outbox
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            log.LogInformation($"Processing outbox message: {message.Id} {message.Type} {message.Data}");
            try
            {
                var messageType = IntegrationEventsAssemblyInfo.Assembly.GetType(message.Type);
                var notification = ((INotification)JsonConvert.DeserializeObject(message.Data, messageType))!;
                await publisher.Publish(notification, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return Unit.Value;
    }
}