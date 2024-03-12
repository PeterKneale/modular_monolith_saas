﻿using Micro.Tenants.Application.Users;
using Micro.Tenants.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Micro.Tenants.Infrastructure.Database.Repositories;

internal class UserRepository(Db db) : IUserRepository
{
    public async Task CreateAsync(User user, CancellationToken token)
    {
        await db.Users.AddAsync(user, token);
    }

    public async Task<User?> GetAsync(UserId id, CancellationToken token)
    {
        return await db.Users
            .Include(x => x.Memberships)
            .Include(x => x.UserApiKeys)
            .SingleOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task<User?> GetAsync(EmailAddress email, CancellationToken token)
    {
        return await db.Users
            .Include(x => x.Memberships)
            .Include(x => x.UserApiKeys)
            .SingleOrDefaultAsync(x => x.Credentials.Email.Equals(email), token);
    }
}