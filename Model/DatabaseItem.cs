﻿using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class DatabaseItem
{
    public int Id { get; set; }

    public string? Letters { get; set; }

    public int? Order { get; set; }

    public string? Image { get; set; }

    public string? Sound { get; set; }

    public int? Type { get; set; }

    public int? UnitId { get; set; }

    public int? RewardType { get; set; }

    public string? Reward { get; set; }

    public int? Grouping { get; set; }

    public DateTime? LastModified { get; set; }

    public string? Text { get; set; }

    public bool? Deleted { get; set; }

    public int? Version { get; set; }

    public string? Loginid { get; set; }

    public string? UniqueId { get; set; }
}
