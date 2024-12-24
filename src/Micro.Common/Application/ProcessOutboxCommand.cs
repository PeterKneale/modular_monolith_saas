using FluentValidation;

namespace Micro.Common.Application;

public class ProcessOutboxCommand : IRequest;

public class ProcessOutboxCommandValidator : AbstractValidator<ProcessOutboxCommand>;