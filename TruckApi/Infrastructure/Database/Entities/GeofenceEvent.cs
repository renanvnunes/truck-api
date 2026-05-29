using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TruckApi.Infrastructure.Database.Entities;

public enum GeofenceEventType
{
    Enter,
    Exit,
}

[Table("geofence_events")]
[Index(nameof(GeofenceId), Name = "geofence_events_geofence_id_idx")]
[Index(nameof(MachineId), Name = "geofence_events_machine_id_idx")]
[Index(nameof(MachineId), nameof(OccurredAt), Name = "geofence_events_machine_time_idx")]
public class GeofenceEvent
{
    [Column("id")]
    public long Id { get; set; }

    [Column("geofence_id")]
    public required string GeofenceId { get; set; }

    public Geofence Geofence { get; set; } = null!;

    [Column("machine_id")]
    public required string MachineId { get; set; }

    public Machine Machine { get; set; } = null!;

    [Column("event_type")]
    public GeofenceEventType EventType { get; set; }

    [Column("occurred_at")]
    public DateTimeOffset OccurredAt { get; set; }

    [Column("latitude")]
    public double Latitude { get; set; }

    [Column("longitude")]
    public double Longitude { get; set; }
}
