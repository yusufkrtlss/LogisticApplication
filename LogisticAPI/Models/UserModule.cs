using System;
using System.Collections.Generic;

namespace LogisticApi.Models;

public partial class UserModule
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ModuleId { get; set; }

    public Guid CompanyId { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Module Module { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
