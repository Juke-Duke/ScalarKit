using System.Text.RegularExpressions;
using ScalarKit.Exceptions;

namespace ScalarKit;

public readonly record struct RGBA : IScalar<RGBA, string>
{
    private static readonly Regex VALID_CRITERIA = new Regex(@"^rgba\(\s*(-?\d+|-?\d*\.\d+(?=%))(%?)\s*,\s*(-?\d+|-?\d*\.\d+(?=%))(\2)\s*,\s*(-?\d+|-?\d*\.\d+(?=%))(\2)\s*,\s*(-?\d+|-?\d*.\d+)\s*\)$");

    public byte RedComponent { get; }

    public byte GreenComponent { get; }

    public byte BlueComponent { get; }

    public Percentage Alpha { get; }

    public string Value => $"rgba({RedComponent}, {GreenComponent}, {BlueComponent}, {Alpha})";

    private RGBA(string rgba)
    {
        var components = rgba.Split('(', ')', ',');

        RedComponent = byte.Parse(components[1]);

        GreenComponent = byte.Parse(components[2]);

        BlueComponent = byte.Parse(components[3]);

        Alpha = byte.Parse(components[4]);
    }

    private RGBA(byte redComponent, byte greenComponent, byte blueComponent, Percentage alphaComponent)
        => (RedComponent, GreenComponent, BlueComponent, Alpha) = (redComponent, greenComponent, blueComponent, alphaComponent);

    public static implicit operator RGBA(string rgba)
        => VALID_CRITERIA.IsMatch(rgba)
            ? new RGBA(rgba)
            : throw new FormatException($"{nameof(RGBA)} value must be in the format 'rgba(000, 000, 000, 000%)'.");

    public static implicit operator RGBA((byte redComponent, byte greenComponent, byte blueComponent, Percentage alphaComponent) rgba)
        => new(rgba.redComponent, rgba.greenComponent, rgba.blueComponent, rgba.alphaComponent);

    public static explicit operator RGBA(HexColorCode hexColorCode)
        => new(hexColorCode.RedComponent, hexColorCode.GreenComponent, hexColorCode.BlueComponent, Math.Round(hexColorCode.Alpha / 255.0));

    public static explicit operator RGBA(RGB rgb)
        => new(rgb.RedComponent, rgb.GreenComponent, rgb.BlueComponent, (Percentage)"100%");

    public static explicit operator RGBA(HSL hsl)
        => (RGBA)(RGB)hsl;

    public static explicit operator RGBA(HSLA hsla)
    {
        var (redComponent, greenComponent, blueComponent) = (RGB)hsla;

        return new(redComponent, greenComponent, blueComponent, hsla.Alpha);
    }

    public void Deconstruct(out byte redComponent, out byte greenComponent, out byte blueComponent, out Percentage alphaComponent)
        => (redComponent, greenComponent, blueComponent, alphaComponent) = (RedComponent, GreenComponent, BlueComponent, Alpha);

    public override string ToString()
        => Value;
}
