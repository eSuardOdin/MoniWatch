using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoniWatch.Entities;

public partial class Moni
{
    [Key]
    public int MoniId { get; set; }

    public string MoniLogin { get; set; } = null!;

    public string MoniPwd { get; set; } = null!;

    [InverseProperty("Moni")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [InverseProperty("Moni")]
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
