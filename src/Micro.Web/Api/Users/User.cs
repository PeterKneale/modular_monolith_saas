using Micro.Users;
using Micro.Users.Application.Users.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Micro.Web.Api.Users;

public class User
{
    public static async Task<Results<Ok<UserDto>, NotFound>> GetCurrentUser(IUsersModule module, ILogger<User> log)
    {
        try
        {
            var query = new GetCurrentUser.Query();
            var result = await module.SendQuery(query);
            return TypedResults.Ok(new UserDto
            {
                UserId = result
            });
        }
        catch (NotFoundException)
        {
            return TypedResults.NotFound();
        }
    }
}