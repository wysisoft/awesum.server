using awesum.server.Model;

internal class GetFollowerFollowRequestsResponse
{
    public IQueryable<Follower> followers { get; }

    public GetFollowerFollowRequestsResponse(IQueryable<Follower> followers)
    {
        this.followers = followers;
    }
}