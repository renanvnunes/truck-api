using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TruckApi.Infrastructure.Database.Entities;

[Table("geofences")]
[Index(nameof(CompanyId), Name = "geofences_company_id_idx")]
public class Geofence
{
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("name")]
    public required string Name { get; set; }

    [Column("company_id")]
    public required string CompanyId { get; set; }

    public Company Company { get; set; } = null!;

    // Polígono em formato GeoJSON
    [Column("polygon", TypeName = "jsonb")]
    public required string Polygon { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    public ICollection<GeofenceEvent> Events { get; set; } = [];

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
