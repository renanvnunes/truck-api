using Microsoft.EntityFrameworkCore;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Machine> Machines => Set<Machine>();
    public DbSet<ChecklistTemplate> ChecklistTemplates => Set<ChecklistTemplate>();
    public DbSet<ChecklistTemplateItem> ChecklistTemplateItems => Set<ChecklistTemplateItem>();
    public DbSet<Checklist> Checklists => Set<Checklist>();
    public DbSet<ChecklistItemResponse> ChecklistItemResponses => Set<ChecklistItemResponse>();
    public DbSet<HourRecord> HourRecords => Set<HourRecord>();
    public DbSet<MachineLocation> MachineLocations => Set<MachineLocation>();
    public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();
    public DbSet<Geofence> Geofences => Set<Geofence>();
    public DbSet<GeofenceEvent> GeofenceEvents => Set<GeofenceEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Whatsapp).IsUnique();
            e.HasIndex(u => u.Document).IsUnique();
            e.Property(u => u.Role).HasConversion<string>();
            e.HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
            e.ToTable(t =>
                t.HasCheckConstraint(
                    "ck_users_company_id_by_role",
                    "(role = 'Admin' AND company_id IS NULL) OR (role <> 'Admin' AND company_id IS NOT NULL)"
                )
            );
        });

        modelBuilder.Entity<Machine>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Type).HasConversion<string>();
            e.Property(m => m.Status).HasConversion<string>();
            e.HasOne(m => m.Company)
                .WithMany(c => c.Machines)
                .HasForeignKey(m => m.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ChecklistTemplate>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.MachineType).HasConversion<string>();
            e.HasOne(t => t.Company)
                .WithMany(c => c.ChecklistTemplates)
                .HasForeignKey(t => t.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ChecklistTemplateItem>(e =>
        {
            e.HasKey(i => i.Id);
            e.HasOne(i => i.Template)
                .WithMany(t => t.Items)
                .HasForeignKey(i => i.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Checklist>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Result).HasConversion<string>();
            e.HasOne(c => c.Machine)
                .WithMany(m => m.Checklists)
                .HasForeignKey(c => c.MachineId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(c => c.Operator)
                .WithMany()
                .HasForeignKey(c => c.OperatorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ChecklistItemResponse>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.Status).HasConversion<string>();
            e.HasOne(r => r.Checklist)
                .WithMany(c => c.ItemResponses)
                .HasForeignKey(r => r.ChecklistId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(r => r.TemplateItem)
                .WithMany()
                .HasForeignKey(r => r.TemplateItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<HourRecord>(e =>
        {
            e.HasKey(h => h.Id);
            e.HasOne(h => h.Machine)
                .WithMany(m => m.HourRecords)
                .HasForeignKey(h => h.MachineId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(h => h.Operator)
                .WithMany()
                .HasForeignKey(h => h.OperatorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MachineLocation>(e =>
        {
            e.HasKey(l => l.Id);
            e.Property(l => l.Id).ValueGeneratedOnAdd();
            e.HasOne(l => l.Machine)
                .WithMany(m => m.Locations)
                .HasForeignKey(l => l.MachineId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MaintenanceRecord>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Type).HasConversion<string>();
            e.HasOne(m => m.Machine)
                .WithMany(m => m.MaintenanceRecords)
                .HasForeignKey(m => m.MachineId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(m => m.PerformedBy)
                .WithMany()
                .HasForeignKey(m => m.PerformedById)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Geofence>(e =>
        {
            e.HasKey(g => g.Id);
            e.HasOne(g => g.Company)
                .WithMany(c => c.Geofences)
                .HasForeignKey(g => g.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<GeofenceEvent>(e =>
        {
            e.HasKey(ev => ev.Id);
            e.Property(ev => ev.Id).ValueGeneratedOnAdd();
            e.Property(ev => ev.EventType).HasConversion<string>();
            e.HasOne(ev => ev.Geofence)
                .WithMany(g => g.Events)
                .HasForeignKey(ev => ev.GeofenceId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(ev => ev.Machine)
                .WithMany()
                .HasForeignKey(ev => ev.MachineId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
