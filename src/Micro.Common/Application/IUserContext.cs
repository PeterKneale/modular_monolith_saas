using Micro.Common.Domain;

namespace Micro.Common.Application;

public interface IUserContext
{
    UserId UserId { get; }
}