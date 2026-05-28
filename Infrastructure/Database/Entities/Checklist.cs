using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruckApi.Infrastructure.Database.Entities;

public enum ChecklistResult
{
    Approved,                  // Aprovada para operação
    ApprovedWithObservations,  // Aprovada com observações
    Rejected                   // Reprovada — bloqueia uso
}

[Table("checklists")]
[Index(nameof(MachineId), Name = "checklists_machine_id_idx")]
[Index(nameof(OperatorId), Name = "checklists_operator_id_idx")]
[Index(nameof(MachineId), nameof(StartedAt), Name = "checklists_machine_date_idx")]
public class Checklist
{
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("machine_id")]
    public required string MachineId { get; set; }

    public Machine Machine { get; set; } = null!;

    [Column("operator_id")]
    public required string OperatorId { get; set; }

    public User Operator { get; set; } = null!;

    [Column("template_id")]
    public required string TemplateId { get; set; }

    public ChecklistTemplate Template { get; set; } = null!;

    [Column("result")]
    public ChecklistResult Result { get; set; }

    [Column("started_at")]
    public DateTimeOffset StartedAt { get; set; }

    [Column("completed_at")]
    public DateTimeOffset? CompletedAt { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    public ICollection<ChecklistItemResponse> ItemResponses { get; set; } = [];

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
