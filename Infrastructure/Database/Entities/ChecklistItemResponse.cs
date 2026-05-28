using System.ComponentModel.DataAnnotations.Schema;

namespace TruckApi.Infrastructure.Database.Entities;

public enum ChecklistItemStatus { Ok, WithObservation, Failed }

[Table("checklist_item_responses")]
public class ChecklistItemResponse
{
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("checklist_id")]
    public required string ChecklistId { get; set; }

    public Checklist Checklist { get; set; } = null!;

    [Column("template_item_id")]
    public required string TemplateItemId { get; set; }

    public ChecklistTemplateItem TemplateItem { get; set; } = null!;

    [Column("status")]
    public ChecklistItemStatus Status { get; set; }

    [Column("observation")]
    public string? Observation { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}
