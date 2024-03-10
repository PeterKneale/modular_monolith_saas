namespace Micro.Common;

public interface IModule
{
    Task SendCommand(IRequest command);

    Task<TResult> SendQuery<TResult>(IRequest<TResult> query);
}