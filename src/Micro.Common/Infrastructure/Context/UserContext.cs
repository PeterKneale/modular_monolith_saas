using Micro.Common.Application;
using Micro.Common.Domain;

namespace Micro.Common.Infrastructure.Context;

public class UserContext(UserId userId) : IUserContext
{
    public UserId UserId => userId;
}