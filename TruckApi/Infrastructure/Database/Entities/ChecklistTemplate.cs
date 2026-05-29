using System.ComponentModel.DataAnnotations.Schema;

namespace TruckApi.Infrastructure.Database.Entities;

[Table("checklist_templates")]
public class ChecklistTemplate
{
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("name")]
    public required string Name { get; set; }

    // Null = template genérico para qualquer tipo de máquina
    [Column("machine_type")]
    public MachineType? MachineType { get; set; }

    [Column("company_id")]
    public required string CompanyId { get; set; }

    public Company Company { get; set; } = null!;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    public ICollection<ChecklistTemplateItem> Items { get; set; } = [];

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
