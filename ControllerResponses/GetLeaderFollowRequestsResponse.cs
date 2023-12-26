using awesum.server.Model;

internal class GetLeaderFollowRequestsResponse
{
    public IQueryable<Follower> followers;

    public GetLeaderFollowRequestsResponse(IQueryable<Follower> followers)
    {
        this.followers = followers;
    }
}