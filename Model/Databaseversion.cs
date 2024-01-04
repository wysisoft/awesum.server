using System;
using System.Collections.Generic;

namespace awesum.server.Model;

public partial class Databaseversion
{
    public int Id { get; set; }

    public int Databaseid { get; set; }

    public string Databasejson { get; set; } = null!;

    public DateTime Updatedate { get; set; }

    public int? Appid { get; set; }
}
