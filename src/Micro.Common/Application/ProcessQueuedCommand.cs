using FluentValidation;

namespace Micro.Common.Application;

public class ProcessQueuedCommand : IRequest
{
}

public class ProcessCommandsValidator : AbstractValidator<ProcessQueuedCommand>
{
}