using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class DatabaseItem
{
    public string Letters { get; set; } = "";

    public int Order { get; set; } = 0;

    public string Image { get; set; } = "";

    public string Sound { get; set; } = "";

    public int Type { get; set; } = 0;

    public int UnitId { get; set; } = 0;

    public int RewardType { get; set; } = 0;

    public string Reward { get; set; } = "";

    public int Grouping { get; set; } = 0;

    public DateTime LastModified { get; set; } = DateTime.Parse("1900-01-01");

    public string Text { get; set; } = "";

    public bool Deleted { get; set; } = false;

    public int Version { get; set; } = 0;

    public string Loginid { get; set; } = "";

    public int DatabaseId { get; set; } = 0;

    public int AppId { get; set; } = 0;

    public string AppUniqueId { get; set; } = "";

    public string UnitUniqueId { get; set; } = "";

    public Guid UniqueId { get; set; } = Guid.Empty;

    public int Id { get; set; } = 0;
}
