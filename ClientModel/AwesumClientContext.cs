using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace awesum.server.ClientModel;

public partial class AwesumClientContext : DbContext
{
    public AwesumClientContext()
    {
    }

    public AwesumClientContext(DbContextOptions<AwesumClientContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppProperty> AppProperties { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=awesum.client;Username=postgres;Password=This4Now!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppProperty>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("appProperties");

            entity.Property(e => e.AppDb1appProperties2manualId).HasColumnName("appDB1appProperties2manualId");
            entity.Property(e => e.AppDb1appProperties2successSound).HasColumnName("appDB1appProperties2successSound");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
