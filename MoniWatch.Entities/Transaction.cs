using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoniWatch.Entities;

public partial class Transaction
{
    [Key]
    public int TransactionId { get; set; }

    public int AccountId { get; set; }

    public double TransactionAmount { get; set; }

    public string TransactionName { get; set; } = null!;

    [Column(TypeName = "DATE")]
    public DateTime TransactionDate { get; set; } = default;

    public int TagId { get; set; }

    [ForeignKey("TagId")]
    [InverseProperty("Transactions")]
    public virtual Tag? Tag { get; set; } = null!;
}
