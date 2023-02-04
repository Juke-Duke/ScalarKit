using System.Text.RegularExpressions;
using ScalarKit.Exceptions;

namespace ScalarKit;

public readonly record struct RGB : IScalar<RGB, string>
{
    private static readonly Regex VALID_CRITERIA = new Regex(@"^rgb\(\s*(-?\d+|-?\d*\.\d+(?=%))(%?)\s*,\s*(-?\d+|-?\d*\.\d+(?=%))(\2)\s*,\s*(-?\d+|-?\d*\.\d+(?=%))(\2)\s*\)$");

    public byte RedComponent { get; }

    public byte GreenComponent { get; }

    public byte BlueComponent { get; }

    public string Value => $"rgb({RedComponent}, {GreenComponent}, {BlueComponent})";

    private RGB(string rgb)
    {
        var components = rgb.Split('(', ')', ',');

        RedComponent = byte.Parse(components[1]);

        GreenComponent = byte.Parse(components[2]);

        BlueComponent = byte.Parse(components[3]);
    }

    private RGB(byte redComponent, byte greenComponent, byte blueComponent)
        => (RedComponent, GreenComponent, BlueComponent) = (redComponent, greenComponent, blueComponent);

    public static implicit operator RGB(string rgb)
        => VALID_CRITERIA.IsMatch(rgb)
            ? new RGB(rgb)
            : throw new FormatException($"{nameof(RGB)} value must be in the format 'rgb(000, 000, 000)'.");

    public static implicit operator RGB((byte redComponent, byte greenComponent, byte blueComponent) rgb)
        => new(rgb.redComponent, rgb.greenComponent, rgb.blueComponent);

    public static explicit operator RGB(HexColorCode hexColorCode)
        => new(hexColorCode.RedComponent, hexColorCode.GreenComponent, hexColorCode.BlueComponent);

    public static explicit operator RGB(RGBA rgba)
        => new(rgba.RedComponent, rgba.GreenComponent, rgba.BlueComponent);

    public static explicit operator RGB(HSL hsl)
    {
        var chroma = (1 - Math.Abs((2 * hsl.Luminance.Value) - 1)) * hsl.Saturation.Value;
        var huePrime = hsl.Hue.Value / 60;
        var x = chroma * (1 - Math.Abs(huePrime % 2 - 1));
        (double redComponent, double greenComponent, double blueComponent) = huePrime switch
        {
            >= 0 and <= 1 => (chroma, x, 0.0),
            >= 1 and <= 2 => (x, chroma, 0.0),
            >= 2 and <= 3 => (0.0, chroma, x),
            >= 3 and <= 4 => (0.0, x, chroma),
            >= 4 and <= 5 => (x, 0.0, chroma),
            >= 5 and <= 6 => (chroma, 0.0, x),
            _ => throw new Exception()
        };
        double m = hsl.Luminance.Value - chroma / 2;
        return new RGB((byte)(Math.Round((redComponent + m) * 255, MidpointRounding.AwayFromZero)),
                       (byte)(Math.Round((greenComponent + m) * 255, MidpointRounding.AwayFromZero)),
                       (byte)(Math.Round((blueComponent + m) * 255, MidpointRounding.AwayFromZero)));
    }

    public static explicit operator RGB(HSLA hsla)
        => (RGB)(HSL)hsla;

    public void Deconstruct(out byte redComponent, out byte greenComponent, out byte blueComponent)
        => (redComponent, greenComponent, blueComponent) = (RedComponent, GreenComponent, BlueComponent);

    public override string ToString()
        => Value;
}
