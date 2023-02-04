using System.Text.RegularExpressions;

namespace ScalarKit;

public readonly record struct HSL : IScalar<HSL, string>
{
    private static readonly Regex VALID_CRITERIA = new Regex(@"^hsl\(\s*(-?\d+|-?\d*.\d+)\s*,\s*(-?\d+|-?\d*.\d+)%\s*,\s*(-?\d+|-?\d*.\d+)%\s*\)$");

    public Degree Hue { get; }

    public Percentage Saturation { get; }

    public Percentage Luminance { get; }

    public string Value => $"hsl({Hue.ToString().TrimEnd('°')}, {Saturation}, {Luminance})";

    private HSL(string hsl)
    {
        var components = hsl.Split('(', ')', ',');

        Hue = double.Parse(components[1]);

        Saturation = (Percentage)components[2];

        Luminance = (Percentage)components[3];
    }

    private HSL(Degree hue, Percentage saturation, Percentage lightness)
        => (Hue, Saturation, Luminance) = (hue, saturation, lightness);

    public static implicit operator HSL(string hsl)
        => VALID_CRITERIA.IsMatch(hsl)
            ? new HSL(hsl)
            : throw new FormatException($"{nameof(HSL)} value must be in the format 'hsl(000, 00%, 00%)'.");

    public static implicit operator HSL((Degree hue, Percentage saturation, Percentage lightness) hsl)
        => new(hsl.hue, hsl.saturation, hsl.lightness);

    public static explicit operator HSL(HexColorCode hexColorCode)
        => (HSL)(RGB)hexColorCode;

    public static explicit operator HSL(RGB rgb)
    {
        var redComponent = rgb.RedComponent / 255.0;
        var greenComponent = rgb.GreenComponent / 255.0;
        var blueComponent = rgb.BlueComponent / 255.0;

        var min = Math.Min(Math.Min(redComponent, greenComponent), blueComponent);
        var max = Math.Max(Math.Max(redComponent, greenComponent), blueComponent);
        var delta = max - min;

        Percentage luminance = (max + min) / 2;
        Degree hue = 0.0;
        Percentage saturation = 0.0;

        if (delta is 0)
            return new(0, 0, luminance);

        if (redComponent == max)
            hue = 60 * (((greenComponent - blueComponent) / delta) % 6);
        else if (greenComponent == max)
            hue = 60 * (((blueComponent - redComponent) / delta) + 2);
        else if (blueComponent == max)
            hue = 60 * (((redComponent - greenComponent) / delta) + 4);

        if (delta > 0 || delta < 0)
            saturation = delta / (1 - Math.Abs(2 * luminance.Value - 1));

        return new(hue, saturation, luminance);
    }

    public static explicit operator HSL(RGBA rgba)
        => (HSL)(RGB)rgba;

    public static explicit operator HSL(HSLA hsla)
        => new(hsla.Hue, hsla.Saturation, hsla.Luminance);

    public void Deconstruct(out Degree hue, out Percentage saturation, out Percentage luminance)
        => (hue, saturation, luminance) = (Hue, Saturation, Luminance);

    public override string ToString()
        => Value;
}
