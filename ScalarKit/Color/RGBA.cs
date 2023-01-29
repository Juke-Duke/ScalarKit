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

    public string Value { get; }

    private RGBA(string rgba)
    {
        Value = rgba;

        var components = rgba.Split('(', ')', ',');

        RedComponent = byte.Parse(components[1]);

        GreenComponent = byte.Parse(components[2]);

        BlueComponent = byte.Parse(components[3]);

        AlphaComponent = byte.Parse(components[4]);
    }

    public static implicit operator RGBA(string rgba)
        => VALID_CRITERIA.IsMatch(rgba)
            ? new RGBA(rgba)
            : throw new InvalidRGBAException(rgba);
}
