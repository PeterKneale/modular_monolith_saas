using Micro.Tenants.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Micro.Tenants.Infrastructure.Integration;

public class ProcessInboxCommandHandler(Db db, IPublisher publisher, ILogger<ProcessInboxCommandHandler> log) : IRequestHandler<ProcessInboxCommand>
{
    public async Task<Unit> Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
    {
        var messages = await db.Inbox
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            log.LogInformation($"Processing inbox message: {message.Id}");
            try
            {
                var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(assembly => message.Type.Contains(assembly.GetName().Name));
                var messageType = messageAssembly.GetType(message.Type);
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