using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruckApi.Infrastructure.Database.Entities;

public enum MachineType
{
    Excavator,       // Escavadeira
    Grader,          // Patrol / Motoniveladora
    Backhoe,         // Retroescavadeira
    Tractor,         // Trator
    Bulldozer,
    Crane,           // Guindaste
    Forklift,        // Empilhadeira
    Truck,           // Caminhão
    Other
}

public enum MachineStatus
{
    Active,            // Ativa
    UnderMaintenance,  // Em manutenção
    Stopped            // Parada
}

[Table("machines")]
[Index(nameof(CompanyId), Name = "machines_company_id_idx")]
[Index(nameof(Status), Name = "machines_status_idx")]
[Index(nameof(SerialNumber), IsUnique = true)]
public class Machine
{
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("code")]
    public required string Code { get; set; }

    [Column("type")]
    public MachineType Type { get; set; }

    [Column("brand")]
    public required string Brand { get; set; }

    [Column("model")]
    public required string Model { get; set; }

    [Column("year")]
    public int Year { get; set; }

    [Column("serial_number")]
    public required string SerialNumber { get; set; }

    [Column("plate")]
    public string? Plate { get; set; }

    [Column("current_hourmeter")]
    public decimal CurrentHourmeter { get; set; } = 0;

    [Column("status")]
    public MachineStatus Status { get; set; } = MachineStatus.Active;

    [Column("company_id")]
    public required string CompanyId { get; set; }

    public Company Company { get; set; } = null!;

    public ICollection<Checklist> Checklists { get; set; } = [];
    public ICollection<HourRecord> HourRecords { get; set; } = [];
    public ICollection<MachineLocation> Locations { get; set; } = [];
    public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = [];

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
