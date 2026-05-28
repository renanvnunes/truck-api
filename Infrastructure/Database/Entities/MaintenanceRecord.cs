using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruckApi.Infrastructure.Database.Entities;

public enum MaintenanceType { Preventive, Corrective }

[Table("maintenance_records")]
[Index(nameof(MachineId), Name = "maintenance_records_machine_id_idx")]
public class MaintenanceRecord
{
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("machine_id")]
    public required string MachineId { get; set; }

    public Machine Machine { get; set; } = null!;

    [Column("type")]
    public MaintenanceType Type { get; set; }

    [Column("description")]
    public required string Description { get; set; }

    [Column("performed_by_id")]
    public string? PerformedById { get; set; }

    public User? PerformedBy { get; set; }

    [Column("performed_at")]
    public DateTimeOffset PerformedAt { get; set; }

    [Column("hourmeter_at_service")]
    public decimal? HourmeterAtService { get; set; }

    [Column("next_service_hourmeter")]
    public decimal? NextServiceHourmeter { get; set; }

    [Column("cost")]
    public decimal? Cost { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
