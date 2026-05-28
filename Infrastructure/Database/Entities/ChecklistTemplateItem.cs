using System.ComponentModel.DataAnnotations.Schema;

namespace TruckApi.Infrastructure.Database.Entities;

[Table("checklist_template_items")]
public class ChecklistTemplateItem
{
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("template_id")]
    public required string TemplateId { get; set; }

    public ChecklistTemplate Template { get; set; } = null!;

    [Column("name")]
    public required string Name { get; set; }

    [Column("order")]
    public int Order { get; set; }

    [Column("is_required")]
    public bool IsRequired { get; set; } = true;

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}
