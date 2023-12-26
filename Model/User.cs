using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Loginid { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<App> Apps { get; set; } = new List<App>();
}
