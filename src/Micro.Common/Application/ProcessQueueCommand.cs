using FluentValidation;

namespace Micro.Common.Application;

public class ProcessQueueCommand : IRequest;

public class ProcessQueueCommandValidator : AbstractValidator<ProcessQueueCommand>;