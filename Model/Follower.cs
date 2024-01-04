using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class Follower
{
    public int Id { get; set; }

    public int? LeaderAppId { get; set; }

    public int? FollowerAppId { get; set; }

    public string? FollowerName { get; set; }

    public string? FollowerEmail { get; set; }

    public string? LeaderName { get; set; }

    public string? LeaderEmail { get; set; }

    public DateTime? LastModified { get; set; }

    public bool? LeaderAccepted { get; set; }

    public bool? LeaderRemoved { get; set; }

    public Guid? UniqueId { get; set; }

    public bool? Deleted { get; set; }
}
