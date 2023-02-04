using System.Text.RegularExpressions;

namespace ScalarKit;

public readonly record struct Duration : IScalar<Duration, string>
{
    private static readonly Regex VALID_CRITERIA = new Regex(@"^(-?)P(?=\d|T\d)(?:(\d+)Y)?(?:(\d+)M)?(?:(\d+)([DW]))?(?:T(?:(\d+)H)?(?:(\d+)M)?(?:(\d+(?:\.\d+)?)S)?)?$");

    public uint Years => uint.Parse(VALID_CRITERIA.Match(Value).Groups[2].Value);

    public uint Months => uint.Parse(VALID_CRITERIA.Match(Value).Groups[3].Value);

    public uint Weeks => uint.Parse(VALID_CRITERIA.Match(Value).Groups[4].Value);

    public uint Days => uint.Parse(VALID_CRITERIA.Match(Value).Groups[4].Value);

    public uint Hours => uint.Parse(VALID_CRITERIA.Match(Value).Groups[6].Value);

    public uint Minutes => uint.Parse(VALID_CRITERIA.Match(Value).Groups[7].Value);

    public double Seconds => double.Parse(VALID_CRITERIA.Match(Value).Groups[8].Value);

    public string Value { get; }

    private Duration(string duration)
        => Value = duration;

    public static implicit operator Duration(string duration)
        => VALID_CRITERIA.IsMatch(duration)
            ? new Duration(duration)
            : throw new FormatException($"{nameof(Duration)} value must be in the format 'P000Y000M000DT000H000M000S'.");
}
