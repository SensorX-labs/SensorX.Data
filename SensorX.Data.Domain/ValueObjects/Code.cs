using System.Text.RegularExpressions;
using SensorX.Data.Domain.Common.Exceptions;

namespace SensorX.Data.Domain.ValueObjects;

public partial record Code
{
    public string Value { get; init; }

    private Code(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Code không được để trống.");
        //kiểm tra định dạng $"{prefix.ToUpper()}-{now:yyMMdd}-{now:HHmmssfff}";
        if (!MyRegex().IsMatch(value))
            throw new DomainException("Định dạng Code không hợp lệ.");
        Value = value;
    }

    public static Code From(string value) => new(value);

    public static Code Create(string prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            throw new DomainException("Tiền tố không được để trống.");

        var now = DateTime.UtcNow;
        var code = $"{prefix.ToUpper()}-{now:yyMMdd}-{now:HHmmssfff}";
        return new Code(code);
    }

    public static implicit operator string(Code code) => code?.Value ?? string.Empty;

    public override string ToString() => Value;
    [GeneratedRegex(@"^[A-Z0-9_-]+$")]
    private static partial Regex MyRegex();
}