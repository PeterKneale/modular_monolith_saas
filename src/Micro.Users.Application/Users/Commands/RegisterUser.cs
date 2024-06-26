﻿using Micro.Common.Infrastructure.Integration.Queue;
using Micro.Users.Application.Users.Queue;
using Micro.Users.Domain.Users.Services;

namespace Micro.Users.Application.Users.Commands;

public static class RegisterUser
{
    public record Command(Guid UserId, string FirstName, string LastName, string Email, string Password) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
            RuleFor(m => m.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(m => m.LastName).NotEmpty().MaximumLength(50);
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
            RuleFor(m => m.Password).NotEmpty().MaximumLength(50);
        }
    }

    public class Handler(IUserRepository users, QueueWriter queue, IHashPassword hasher) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var userId = UserId.Create(command.UserId);
            var userEmail = EmailAddress.Create(command.Email);
            var userName = Name.Create(command.FirstName, command.LastName);
            var userPassword = Password.Create(command.Password);

            if (await users.GetAsync(userId, token) != null) throw new AlreadyExistsException(nameof(User), userId);

            if (await users.GetAsync(userEmail, token) != null) AlreadyExistsException.ThrowBecauseEmailAlreadyExists(nameof(User), userEmail);

            var user = User.Create(userId, userName, userEmail, userPassword, hasher);
            await users.CreateAsync(user, token);

            var sendEmail = new SendWelcomeEmail.Command { UserId = userId };
            await queue.WriteAsync(sendEmail, token);
        }
    }
}