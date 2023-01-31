using System.Text.RegularExpressions;
using ScalarKit.Exceptions;

namespace ScalarKit;

public readonly record struct RGBA : IScalar<RGBA, string>
{
    private static readonly Regex VALID_CRITERIA = new Regex(@"^rgba\(\s*(-?\d+|-?\d*\.\d+(?=%))(%?)\s*,\s*(-?\d+|-?\d*\.\d+(?=%))(\2)\s*,\s*(-?\d+|-?\d*\.\d+(?=%))(\2)\s*,\s*(-?\d+|-?\d*.\d+)\s*\)$");

    public byte RedComponent { get; }

    public byte GreenComponent { get; }

    public byte BlueComponent { get; }

    public byte AlphaComponent { get; }

    public string Value => $"rgba({RedComponent}, {GreenComponent}, {BlueComponent}, {AlphaComponent})";

    private RGBA(string rgba)
    {
        var components = rgba.Split('(', ')', ',');

        RedComponent = byte.Parse(components[1]);

        GreenComponent = byte.Parse(components[2]);

        BlueComponent = byte.Parse(components[3]);

        AlphaComponent = byte.Parse(components[4]);
    }

    private RGBA(byte redComponent, byte greenComponent, byte blueComponent, byte alphaComponent)
        => (RedComponent, GreenComponent, BlueComponent, AlphaComponent) = (redComponent, greenComponent, blueComponent, alphaComponent);

    public static implicit operator RGBA(string rgba)
        => VALID_CRITERIA.IsMatch(rgba)
            ? new RGBA(rgba)
            : throw new InvalidRGBAException(rgba);

    public static implicit operator RGBA((byte redComponent, byte greenComponent, byte blueComponent, byte alphaComponent) rgba)
        => new(rgba.redComponent, rgba.greenComponent, rgba.blueComponent, rgba.alphaComponent);

    public static explicit operator RGBA(HexColorCode hexColorCode)
        => new(hexColorCode.RedComponent, hexColorCode.GreenComponent, hexColorCode.BlueComponent, hexColorCode.AlphaComponent);

    public static explicit operator RGBA(RGB rgb)
        => new(rgb.RedComponent, rgb.GreenComponent, rgb.BlueComponent, 255);

    public void Deconstruct(out byte redComponent, out byte greenComponent, out byte blueComponent, out byte alphaComponent)
        => (redComponent, greenComponent, blueComponent, alphaComponent) = (RedComponent, GreenComponent, BlueComponent, AlphaComponent);

    public override string ToString()
        => Value;
}
