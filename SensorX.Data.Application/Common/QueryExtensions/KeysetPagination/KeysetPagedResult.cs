namespace SensorX.Data.Application.Common.QueryExtensions.KeysetPagination;

public class KeysetPagedResult<T>
{
    public List<T> Items { get; set; } = [];

    public string? LastValue { get; set; }
    public Guid? LastId { get; set; }

    public bool HasNext { get; set; }
}
