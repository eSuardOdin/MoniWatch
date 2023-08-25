using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoniWatch.Entities;

public partial class Tag
{
    [Key]
    public int TagId { get; set; }

    public string TagName { get; set; } = null!;

    public int MoniId { get; set; }

    /* [ForeignKey("MoniId")]
    [InverseProperty("Tags")]
    public virtual Moni? Moni { get; set; } = null!;

    [InverseProperty("Tag")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>(); */
}
