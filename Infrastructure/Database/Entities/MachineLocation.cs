using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TruckApi.Infrastructure.Database.Entities;

[Table("machine_locations")]
[Index(nameof(MachineId), nameof(RecordedAt), Name = "machine_locations_machine_time_idx")]
public class MachineLocation
{
    [Column("id")]
    public long Id { get; set; }

    [Column("machine_id")]
    public required string MachineId { get; set; }

    public Machine Machine { get; set; } = null!;

    [Column("latitude")]
    public double Latitude { get; set; }

    [Column("longitude")]
    public double Longitude { get; set; }

    [Column("recorded_at")]
    public DateTimeOffset RecordedAt { get; set; }

    // Velocidade em km/h (vindo do GPS/IoT)
    [Column("speed")]
    public decimal? Speed { get; set; }

    // Direção em graus (0-360)
    [Column("heading")]
    public decimal? Heading { get; set; }
}
