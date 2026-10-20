using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public sealed class SqlContext : DbContext
{
    public DbSet<Visitor> Visitors { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Attraction> Attractions { get; set; }
    public DbSet<Incidence> Incidences { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<VisitedAttraction> VisitedAttractions { get; set; }
    public DbSet<VisitorParkInformation> VisitorParkInformations { get; set; }
    public DbSet<SpecialEvent> SpecialEvents { get; set; }
    public DbSet<Reward> Rewards { get; set; }
    public DbSet<RewardRedemption> RewardRedemptions { get; set; }
    public DbSet<PointsHistory> PointsHistory { get; set; }

    public SqlContext(DbContextOptions<SqlContext> options)
        : base(options)
    {
        if(!Database.IsInMemory())
        {
            Database.Migrate();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Visitor>()
            .HasKey(v => v.Id);

        modelBuilder.Entity<Visitor>()
            .HasIndex(v => v.Email)
            .IsUnique();

        modelBuilder.Entity<Visitor>()
            .HasOne(v => v.VisitorParkInformation)
            .WithOne()
            .HasForeignKey<VisitorParkInformation>(pi => pi.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VisitorParkInformation>()
            .HasKey(pi => pi.Id);

        modelBuilder.Entity<Worker>()
            .ToTable("Workers")
            .HasKey(w => w.Id);

        modelBuilder.Entity<Worker>()
            .HasIndex(w => w.Email)
            .IsUnique();

        modelBuilder.Entity<Attraction>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<Attraction>()
            .HasIndex(a => a.Name);

        modelBuilder.Entity<Attraction>()
            .Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<Attraction>()
            .Property(a => a.Description)
            .HasMaxLength(500);

        modelBuilder.Entity<Attraction>()
            .Property(a => a.TypeAttraction)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<Incidence>()
            .HasKey(i => i.Id);

        modelBuilder.Entity<Incidence>()
            .Property(i => i.Description)
            .IsRequired()
            .HasMaxLength(500);

        modelBuilder.Entity<Incidence>()
            .Property(i => i.AttractionName)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<Incidence>()
            .Property(i => i.Active)
            .HasDefaultValue(false);

        modelBuilder.Entity<Incidence>()
            .Property(i => i.IsMaintenance)
            .IsRequired()
            .HasDefaultValue(false);

        modelBuilder.Entity<VisitorParkInformation>()
            .HasIndex(vI => vI.NFC)
            .IsUnique();

        modelBuilder.Entity<VisitorParkInformation>()
            .Property(vI => vI.NFC)
            .HasDefaultValueSql("NEWID()");

        modelBuilder.Entity<Ticket>()
            .HasKey(t => t.VisitorParkInformationId);

        modelBuilder.Entity<Ticket>()
            .HasOne<VisitorParkInformation>()
            .WithMany(t => t.Tickets)
            .HasForeignKey(t => t.VisitorParkInformationId);

        modelBuilder.Entity<Ticket>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Ticket>()
            .HasIndex(t => t.QR)
            .IsUnique();

        modelBuilder.Entity<Ticket>()
            .Property(t => t.QR)
            .HasDefaultValueSql("NEWID()");

        modelBuilder.Entity<VisitedAttraction>()
            .HasKey(va => new
            {
                va.VisitorParkInformationId,
                va.AttractionId,
                va.VisitDate
            });

        modelBuilder.Entity<VisitedAttraction>()
            .HasOne<VisitorParkInformation>()
            .WithMany()
            .HasForeignKey(va => va.VisitorParkInformationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VisitedAttraction>()
            .HasOne<Attraction>()
            .WithMany()
            .HasForeignKey(va => va.AttractionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SpecialEvent>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<SpecialEvent>()
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<SpecialEvent>()
            .Property(e => e.Date)
            .IsRequired();

        modelBuilder.Entity<SpecialEvent>()
            .Property(e => e.StartTime)
            .IsRequired();

        modelBuilder.Entity<SpecialEvent>()
            .Property(e => e.EndTime)
            .IsRequired();

        modelBuilder.Entity<SpecialEvent>()
            .Property(e => e.MaxCapacity)
            .IsRequired();

        modelBuilder.Entity<SpecialEvent>()
            .Property(e => e.CurrentCapacity)
            .IsRequired();

        modelBuilder.Entity<SpecialEvent>()
            .Property(e => e.AdditionalCost)
            .HasPrecision(10, 2);

        modelBuilder.Entity<SpecialEvent>()
            .Property(e => e.Active)
            .IsRequired();

        modelBuilder.Entity<SpecialEvent>()
            .HasMany(se => se.IncludedAttractions)
            .WithMany()
            .UsingEntity(j => j.ToTable("specialEvent_Attraction"));

        modelBuilder.Entity<Ticket>()
            .HasOne<SpecialEvent>()
            .WithMany()
            .HasForeignKey(t => t.SpecialEventId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Attraction>()
            .HasIndex(a => a.Name)
            .IsUnique();

        modelBuilder.Entity<Reward>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();
            entity.Property(r => r.Description).IsRequired();
            entity.Property(r => r.PointsCost).IsRequired();
            entity.Property(r => r.AvailableQuantity).IsRequired();
            entity.Property(r => r.Active).IsRequired();
        });

        modelBuilder.Entity<RewardRedemption>(entity =>
        {
            entity.HasKey(rr => rr.Id);
            entity.HasOne<VisitorParkInformation>(rr => rr.VisitorParkInformation)
                .WithMany(vpi => vpi.RewardRedemptions)
                .HasForeignKey(rr => rr.VisitorParkInformationId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Reward>(rr => rr.Reward)
                .WithMany()
                .HasForeignKey(rr => rr.RewardId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<PointsHistory>(entity =>
        {
            entity.HasKey(ph => ph.Id);

            entity.HasOne<VisitorParkInformation>()
                .WithMany()
                .HasForeignKey(ph => ph.VisitorParkInformationId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
