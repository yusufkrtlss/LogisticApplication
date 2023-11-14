using System;
using System.Collections.Generic;

namespace LogisticApi.Models;

public partial class Company
{
    public Guid Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public string TaxNumber { get; set; } = null!;

    public string? Address { get; set; }

    public int FirmNumber { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? LastUpdated { get; set; }

    public Guid? LastUpdatedBy { get; set; }

  
}
