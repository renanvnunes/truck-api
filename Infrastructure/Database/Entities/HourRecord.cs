using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruckApi.Infrastructure.Database.Entities;

[Table("hour_records")]
[Index(nameof(MachineId), Name = "hour_records_machine_id_idx")]
[Index(nameof(OperatorId), Name = "hour_records_operator_id_idx")]
[Index(nameof(MachineId), nameof(Date), Name = "hour_records_machine_date_idx")]
public class HourRecord
{
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("machine_id")]
    public required string MachineId { get; set; }

    public Machine Machine { get; set; } = null!;

    [Column("operator_id")]
    public required string OperatorId { get; set; }

    public User Operator { get; set; } = null!;

    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("started_at")]
    public DateTimeOffset StartedAt { get; set; }

    [Column("ended_at")]
    public DateTimeOffset? EndedAt { get; set; }

    [Column("total_hours")]
    public decimal? TotalHours { get; set; }

    // Leitura do horímetro para validação cruzada
    [Column("hourmeter_start")]
    public decimal? HourmeterStart { get; set; }

    [Column("hourmeter_end")]
    public decimal? HourmeterEnd { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
