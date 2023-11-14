using System;
using System.Collections.Generic;

namespace LogisticApi.Models;

public partial class ProgramParameter
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string ParameterName { get; set; } = null!;

    public string? ParameterValue { get; set; }

    public virtual Company Company { get; set; } = null!;
}
