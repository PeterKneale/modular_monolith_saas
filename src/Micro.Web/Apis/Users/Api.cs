using Micro.Users.Application.Users.Queries;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Micro.Web.Apis.Users;

public class Api
{
    public static async Task<Results<Ok<Dto>, NotFound>> GetCurrentUser(IUsersModule module, ILogger<Api> log)
    {
        try
        {
            var query = new GetCurrentUser.Query();
            var result = await module.SendQuery(query);
            return TypedResults.Ok(new Dto
            {
                UserId = result.Id
            });
        }
        catch (NotFoundException)
        {
            return TypedResults.NotFound();
        }
    }
}