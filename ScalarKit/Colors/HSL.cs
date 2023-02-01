using System.Text.RegularExpressions;

namespace ScalarKit;

public readonly record struct HSL : IScalar<HSL, string>
{
    private static readonly Regex VALID_CRITERIA = new Regex(@"^hsl\(\s*(-?\d+|-?\d*.\d+)\s*,\s*(-?\d+|-?\d*.\d+)%\s*,\s*(-?\d+|-?\d*.\d+)%\s*\)$");

    public Degree Hue { get; }

    public Percentage Saturation { get; }

    public Percentage Lightness { get; }

    public string Value => $"hsl({Hue.ToString().TrimEnd('°')}, {Saturation}, {Lightness})";

    private HSL(string hsl)
    {
        var components = hsl.Split('(', ')', ',');

        Hue = double.Parse(components[1]);

        Saturation = (Percentage)components[2];

        Lightness = (Percentage)components[3];
    }

    private HSL(Degree hue, Percentage saturation, Percentage lightness)
        => (Hue, Saturation, Lightness) = (hue, saturation, lightness);

    public static implicit operator HSL(string hsl)
        => VALID_CRITERIA.IsMatch(hsl)
            ? new HSL(hsl)
            : throw new FormatException($"{nameof(HSL)} value must be in the format 'hsl(000, 00%, 00%)'.");

    public static implicit operator HSL((Degree hue, Percentage saturation, Percentage lightness) hsl)
        => new(hsl.hue, hsl.saturation, hsl.lightness);

    public override string ToString()
        => Value;
}
