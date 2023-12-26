
using awesum.server.Model;

internal class AddFollowRequestResponse
{
    public IQueryable<Follower> followers;

    public AddFollowRequestResponse(IQueryable<Follower> followers)
    {
        this.followers = followers;
    }
}