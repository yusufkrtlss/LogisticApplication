using System;
using System.Collections.Generic;

namespace LogisticApi.Models;

public partial class Module
{
    public Guid Id { get; set; }

    public string ModuleName { get; set; } = null!;

    public string? ModuleNameEn { get; set; }

    public Guid? ParentId { get; set; }

    public int? ScreenOrder { get; set; }

    public virtual ICollection<RoleModule> RoleModules { get; set; } = new List<RoleModule>();

    public virtual ICollection<UserModule> UserModules { get; set; } = new List<UserModule>();
}
