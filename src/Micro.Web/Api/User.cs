using Micro.Users;
using Micro.Users.Application.Users.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Micro.Web.Api;

public class User
{
    public static async Task<Results<Ok<UserDto>, NotFound>> GetCurrentUser(IUsersModule module, ILogger<User> log)
    {
        var query = new GetCurrentUser.Query();
        try
        {
            var result = await module.SendQuery(query);
            return TypedResults.Ok(new UserDto
            {
                UserId = result
            });
        }
        catch (NotFoundException e)
        {
            log.LogError(e, "User not found.");
            return TypedResults.NotFound();
        }
    }
}