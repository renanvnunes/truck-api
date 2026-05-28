using System.ComponentModel.DataAnnotations.Schema;

namespace TruckApi.Infrastructure.Database.Entities;

[Table("companies")]
public class Company
{
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("name")]
    public required string Name { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<User> Users { get; set; } = [];
    public ICollection<Machine> Machines { get; set; } = [];
    public ICollection<ChecklistTemplate> ChecklistTemplates { get; set; } = [];
    public ICollection<Geofence> Geofences { get; set; } = [];
}
