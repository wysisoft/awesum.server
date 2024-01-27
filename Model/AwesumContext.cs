using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace awesum.server.Model;

public partial class AwesumContext : DbContext
{
    public AwesumContext()
    {
    }

    public AwesumContext(DbContextOptions<AwesumContext> options)
        : base(options)
    {
    }

    public virtual DbSet<App> Apps { get; set; }

    public virtual DbSet<Database> Databases { get; set; }

    public virtual DbSet<DatabaseItem> DatabaseItems { get; set; }

    public virtual DbSet<DatabaseType> DatabaseTypes { get; set; }

    public virtual DbSet<DatabaseUnit> DatabaseUnits { get; set; }

    public virtual DbSet<Follower> Followers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=awesum;Username=postgres;Password=This4Now!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<App>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("apps_pkey");

            entity.ToTable("apps");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AllowedToInitiateFollows).HasColumnName("allowedToInitiateFollows");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.HomePageIcon).HasColumnName("homePageIcon");
            entity.Property(e => e.LastModified)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastModified");
            entity.Property(e => e.Loginid).HasColumnName("loginid");
            entity.Property(e => e.ManualId).HasColumnName("manualId");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        modelBuilder.Entity<Database>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("databases");

            entity.Property(e => e.AppId).HasColumnName("appId");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.GroupName).HasColumnName("groupName");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.LastModified)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastModified");
            entity.Property(e => e.Loginid).HasColumnName("loginid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.UniqueId).HasColumnName("uniqueId");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        modelBuilder.Entity<DatabaseItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("databaseItems_pkey");

            entity.ToTable("databaseItems");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AppId).HasColumnName("appId");
            entity.Property(e => e.DatabaseId).HasColumnName("databaseId");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.Grouping).HasColumnName("grouping");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.LastModified)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastModified");
            entity.Property(e => e.Letters).HasColumnName("letters");
            entity.Property(e => e.Loginid).HasColumnName("loginid");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Reward).HasColumnName("reward");
            entity.Property(e => e.RewardType).HasColumnName("rewardType");
            entity.Property(e => e.Sound).HasColumnName("sound");
            entity.Property(e => e.Text).HasColumnName("text");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UniqueId).HasColumnName("uniqueId");
            entity.Property(e => e.UnitId).HasColumnName("unitId");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        modelBuilder.Entity<DatabaseType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("types_pkey");

            entity.ToTable("databaseTypes");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AppId).HasColumnName("appId");
            entity.Property(e => e.DatabaseGroup).HasColumnName("databaseGroup");
            entity.Property(e => e.DatabaseId).HasColumnName("databaseId");
            entity.Property(e => e.LastModified)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastModified");
            entity.Property(e => e.Loginid).HasColumnName("loginid");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UniqueId).HasColumnName("uniqueId");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        modelBuilder.Entity<DatabaseUnit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("databaseProperties_pkey");

            entity.ToTable("databaseUnits");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AppId).HasColumnName("appId");
            entity.Property(e => e.DatabaseId).HasColumnName("databaseId");
            entity.Property(e => e.DatabaseTypeId).HasColumnName("databaseTypeId");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.LastModified)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastModified");
            entity.Property(e => e.Loginid).HasColumnName("loginid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.UniqueId).HasColumnName("uniqueId");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        modelBuilder.Entity<Follower>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("followrequests_pkey");

            entity.ToTable("followers");

            entity.HasIndex(e => e.LeaderAppId, "fki_followrequests_apps");

            entity.HasIndex(e => e.FollowerAppId, "fki_followrequests_followerapp");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DatabaseId).HasColumnName("databaseId");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.FollowerAppId).HasColumnName("followerAppId");
            entity.Property(e => e.FollowerDatabaseGroup).HasColumnName("followerDatabaseGroup");
            entity.Property(e => e.FollowerEmail).HasColumnName("followerEmail");
            entity.Property(e => e.FollowerLoginId).HasColumnName("followerLoginId");
            entity.Property(e => e.FollowerName).HasColumnName("followerName");
            entity.Property(e => e.InitiatedBy).HasColumnName("initiatedBy");
            entity.Property(e => e.LastModified)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastModified");
            entity.Property(e => e.LeaderAccepted).HasColumnName("leaderAccepted");
            entity.Property(e => e.LeaderAppId).HasColumnName("leaderAppId");
            entity.Property(e => e.LeaderEmail).HasColumnName("leaderEmail");
            entity.Property(e => e.LeaderName).HasColumnName("leaderName");
            entity.Property(e => e.LeaderRemoved).HasColumnName("leaderRemoved");
            entity.Property(e => e.UniqueId).HasColumnName("uniqueId");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
