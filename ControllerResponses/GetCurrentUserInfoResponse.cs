using awesum.server.Model;

namespace csharp.Controllers;

internal class GetCurrentUserInfoResponse
{
    public string ManualId { get; set; } = string.Empty;
    public string AuthenticationType { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Id { get; set; } = 0;
    public Guid AppId { get; set; }
    public IQueryable<Follower> Followers { get; set; } = new List<Follower>().AsQueryable();
}