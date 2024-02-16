using Micro.Common.Domain;

namespace Micro.Common.Application;

public interface IUserExecutionContext
{
    UserId UserId { get; }
}