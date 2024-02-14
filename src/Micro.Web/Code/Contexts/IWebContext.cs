namespace Micro.Web.Code.Contexts;

public interface IWebContext
{
    bool IsAuthenticated { get; }
    
    Guid? UserId { get; }

    Guid? OrganisationId { get; }
    string OrganisationName { get; }

    Guid? ProjectId { get; }
}