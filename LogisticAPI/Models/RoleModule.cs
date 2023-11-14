using System;
using System.Collections.Generic;

namespace LogisticApi.Models;

public partial class RoleModule
{
    public Guid Id { get; set; }

    public Guid RoleId { get; set; }

    public Guid ModuleId { get; set; }

    public Guid CompanyId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? LastUpdated { get; set; }

    public Guid? LastUpdatedBy { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Module Module { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
