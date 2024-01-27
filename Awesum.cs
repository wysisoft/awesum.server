using awesum.server.Model;

namespace awesum
{
    class Awesum
    {
        public static int maxAppCount = 100;

        public static IQueryable<Follower> LeaderOrFollowerRows(IQueryable<Follower> query, App foundApp)
        {
            return query.Where(o => o.FollowerAppId == foundApp.Id || o.LeaderAppId == foundApp.Id);
        }
    }
}
