using awesum.server.Model;

namespace csharp.Controllers;

internal class GetCurrentUserInfoResponse
{
    public string? ManualId { get; set;}
    public string? AuthenticationType { get; set;}
    public string Email { get; set;}
    public string Id { get; set;}
    public Guid AppId { get; set;}
    public IQueryable<Follower> Followers { get; set;}
}