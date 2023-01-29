using System.Text.RegularExpressions;
using ScalarKit.Exceptions;

namespace ScalarKit;

public readonly record struct RGB : IScalar<RGB, string>
{
    private static readonly Regex VALID_CRITERIA = new Regex(@"^rgb\(\s*(-?\d+|-?\d*\.\d+(?=%))(%?)\s*,\s*(-?\d+|-?\d*\.\d+(?=%))(\2)\s*,\s*(-?\d+|-?\d*\.\d+(?=%))(\2)\s*\)$");

    public byte RedComponent { get; }

    public byte GreenComponent { get; }

    public byte BlueComponent { get; }

    public string Value { get; }

    private RGB(string rgb)
    {
        Value = rgb;

        var components = rgb.Split('(', ')', ',');

        RedComponent = byte.Parse(components[1]);

        GreenComponent = byte.Parse(components[2]);

        BlueComponent = byte.Parse(components[3]);
    }

    public RGB(byte redComponent, byte greenComponent, byte blueComponent)
        => (RedComponent, GreenComponent, BlueComponent, Value) = (redComponent, greenComponent, blueComponent, $"rgb({redComponent}, {greenComponent}, {blueComponent})");

    public static implicit operator RGB(string rgb)
        => VALID_CRITERIA.IsMatch(rgb)
            ? new RGB(rgb)
            : throw new InvalidRGBException(rgb);
}
