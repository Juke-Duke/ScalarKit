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
            : throw new InvalidRGBException(rgb);

    public static implicit operator RGB((byte redComponent, byte greenComponent, byte blueComponent) rgb)
        => new(rgb.redComponent, rgb.greenComponent, rgb.blueComponent);

    public static explicit operator RGB(HexColorCode hexColorCode)
        => new(hexColorCode.RedComponent, hexColorCode.GreenComponent, hexColorCode.BlueComponent);

    public static explicit operator RGB(RGBA rgba)
        => new(rgba.RedComponent, rgba.GreenComponent, rgba.BlueComponent);

    public void Deconstruct(out byte redComponent, out byte greenComponent, out byte blueComponent)
        => (redComponent, greenComponent, blueComponent) = (RedComponent, GreenComponent, BlueComponent);

    public override string ToString()
        => Value;
}
