namespace SensorX.Data.Domain.SeedWork;

public record VoId(Guid Value)
{
    public static implicit operator Guid(VoId voId) => voId?.Value ?? Guid.Empty;
    public override string ToString() => Value.ToString();
    public static bool operator <(VoId a, VoId b) => a.Value.CompareTo(b.Value) < 0;
    public static bool operator >(VoId a, VoId b) => a.Value.CompareTo(b.Value) > 0;
}