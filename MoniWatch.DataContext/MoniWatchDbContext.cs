using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MoniWatch.Entities;

namespace MoniWatch.DataContext;

public partial class MoniWatchDbContext : DbContext
{
    public MoniWatchDbContext()
    {
    }

    public MoniWatchDbContext(DbContextOptions<MoniWatchDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Moni> Monies { get; set; }

    public virtual DbSet<Snapshot> Snapshots { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Filename=../AppBase.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
