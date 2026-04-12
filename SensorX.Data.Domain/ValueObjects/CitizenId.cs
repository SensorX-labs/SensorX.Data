using System.Text.RegularExpressions;
using SensorX.Data.Domain.Common.Exceptions;

namespace SensorX.Data.Domain.ValueObjects;

public partial record CitizenId
{
    private static readonly Regex CitizenIdRegex = GeneratedCitizenIdRegex();

    public string Value { get; init; }

    private CitizenId(string value) => Value = value;

    public static CitizenId From(string value) => new(value);

    public static CitizenId Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Số CCCD không được để trống.");

        if (!CitizenIdRegex.IsMatch(value))
            throw new DomainException("Số CCCD không hợp lệ (phải gồm 12 chữ số).");

        return new CitizenId(value);
    }

    [GeneratedRegex(@"^\d{12}$")]
    private static partial Regex GeneratedCitizenIdRegex();

    public static implicit operator string(CitizenId id) => id.Value;
}