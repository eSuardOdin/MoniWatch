using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoniWatch.Entities;

public partial class Snapshot
{
    [Key]
    public int SnapshotId { get; set; }

    public int AccountId { get; set; }

    [Column(TypeName = "DATE")]
    public DateTime SnapshotDate { get; set; } = default;

    public double SnapshotBalance { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Snapshots")]
    public virtual Account Account { get; set; } = null!;
}
