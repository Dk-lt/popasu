using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<MaterialItem> MaterialItems { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Furniture> Furniture { get; set; }
    public DbSet<Software> Software { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure MaterialItem inheritance using TPH (Table Per Hierarchy)
        modelBuilder.Entity<MaterialItem>()
            .HasDiscriminator<string>("ItemType")
            .HasValue<Equipment>("Equipment")
            .HasValue<Furniture>("Furniture")
            .HasValue<Software>("Software");

        // Configure MaterialItem base properties
        modelBuilder.Entity<MaterialItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.ReceivedDate).IsRequired();
            entity.Property(e => e.State).IsRequired().HasConversion<string>();
        });

        // Configure Equipment specific properties and relationships
        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.Property(e => e.SerialNumber).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
            entity.Property(e => e.TechnicalCharacteristics).HasMaxLength(1000);
            entity.Property(e => e.ClassroomNumber).HasMaxLength(50);
            
            entity.HasOne(e => e.Classroom)
                .WithMany(c => c.Equipment)
                .HasForeignKey(e => e.ClassroomNumber)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Furniture specific properties and relationships
        modelBuilder.Entity<Furniture>(entity =>
        {
            entity.Property(e => e.Type).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Material).HasMaxLength(200);
            entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ClassroomNumber).HasMaxLength(50);
            
            entity.HasOne(e => e.Classroom)
                .WithMany(c => c.Furniture)
                .HasForeignKey(e => e.ClassroomNumber)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Software specific properties
        modelBuilder.Entity<Software>(entity =>
        {
            entity.Property(e => e.Version).IsRequired().HasMaxLength(50);
            entity.Property(e => e.License).IsRequired().HasMaxLength(200);
            entity.Property(e => e.LicenseExpirationDate).IsRequired();
        });

        // Configure Classroom
        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasKey(e => e.Number);
            entity.Property(e => e.Number).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Capacity).IsRequired();
            entity.Property(e => e.ClassroomType).IsRequired().HasMaxLength(100);
        });

        // Configure User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Role).IsRequired().HasConversion<string>();
            entity.Property(e => e.Login).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(200);
        });
    }
}

