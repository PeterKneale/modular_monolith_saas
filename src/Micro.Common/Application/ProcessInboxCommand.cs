using FluentValidation;

namespace Micro.Common.Application;

public class ProcessInboxCommand : IRequest
{
}

public class ProcessInboxCommandValidator : AbstractValidator<ProcessInboxCommand>
{
}