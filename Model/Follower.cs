using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class Follower
{
    public int Id { get; set; } = 0;

    public int LeaderAppId { get; set; } = 0;

    public int FollowerAppId { get; set; } = 0;

    public string FollowerName { get; set; } = "";

    public string FollowerEmail { get; set; } = "";

    public string LeaderName { get; set; } = "";

    public string LeaderEmail { get; set; } = "";

    public DateTime LastModified { get; set; } = DateTime.Parse("1900-01-01");

    public bool LeaderAccepted { get; set; } = false;

    public bool LeaderRemoved { get; set; } = false;

    public Guid UniqueId { get; set; } = Guid.Empty;

    public bool Deleted { get; set; } = false;

    public string FollowerLoginId { get; set; } = "";

    public int Version { get; set; } = 0;

    public int DatabaseId { get; set; } = 0;

    public int InitiatedBy { get; set; } = 0;

    public string FollowerDatabaseGroup { get; set; } = "";
}
