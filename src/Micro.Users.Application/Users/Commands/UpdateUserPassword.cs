﻿using Micro.Users.Domain.Users.Services;

namespace Micro.Users.Application.Users.Commands;

public static class UpdateUserPassword
{
    public record Command(string OldPassword, string NewPassword) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.OldPassword).NotEmpty().MaximumLength(50);
            RuleFor(m => m.NewPassword).NotEmpty().MaximumLength(50);
        }
    }

    public class Handler(IExecutionContext context, IUserRepository users, ICheckPassword checker, IHashPassword hasher) : IRequestHandler<Command>
    {
        public async Task Handle(Command command, CancellationToken token)
        {
            var userId = context.UserId;

            var user = await users.GetAsync(userId, token);
            if (user == null) throw new NotFoundException(nameof(User), userId.Value);

            var oldPassword = Password.Create(command.OldPassword);
            var newPassword = Password.Create(command.NewPassword);

            user.ChangePassword(oldPassword, newPassword, checker, hasher);
            users.Update(user);
        }
    }
}