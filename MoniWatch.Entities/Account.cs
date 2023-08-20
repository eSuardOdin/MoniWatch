using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoniWatch.Entities;

public partial class Account
{
    [Key]
    public int AccountId { get; set; }

    public string AccountName { get; set; } = null!;

    public int MoniId { get; set; }

    public double AccountBalance { get; set; }

    [ForeignKey("MoniId")]
    [InverseProperty("Accounts")] // Warning, may need to delete null forgiving
    public virtual Moni? Moni { get; set; } = null!;

    [InverseProperty("Account")]
    public virtual ICollection<Snapshot> Snapshots { get; set; } = new List<Snapshot>();
}
