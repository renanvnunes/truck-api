namespace TruckApi.Shared.Utils;

public static class IdGenerator
{
    public static string New() => Ulid.NewUlid().ToString().ToLower();
}
