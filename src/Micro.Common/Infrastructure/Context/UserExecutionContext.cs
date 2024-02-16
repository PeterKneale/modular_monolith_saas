using Micro.Common.Application;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Context;

public class UserExecutionContext(UserId userId) : IUserExecutionContext
{
    public UserId UserId => userId;
}