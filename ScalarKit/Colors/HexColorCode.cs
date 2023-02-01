using System.Text.RegularExpressions;
using ScalarKit.Exceptions;

namespace ScalarKit;

public readonly record struct HexColorCode : IScalar<HexColorCode, string>
{
    private static readonly Regex VALID_HEX_CRITERIA = new Regex(@"#([A-Fa-f0-9]{3,4}|[A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$");

    private static string Format => Lowercase ? "x2" : "X2";

    public static bool Lowercase = false;

    public byte RedComponent { get; }

    public byte GreenComponent { get; }

    public byte BlueComponent { get; }

    public byte AlphaComponent { get; }

    public string Value => $"#{RedComponent.ToString(Format)}{GreenComponent.ToString(Format)}{BlueComponent.ToString(Format)}{AlphaComponent.ToString(Format)}";

    private HexColorCode(string hexColorCode)
    {
        var components = hexColorCode.Substring(1);
        if (components.Length is 3 or 4)
        {
            RedComponent = byte.Parse(components[0].ToString(), System.Globalization.NumberStyles.HexNumber);
            GreenComponent = byte.Parse(components[1].ToString(), System.Globalization.NumberStyles.HexNumber);
            BlueComponent = byte.Parse(components[2].ToString(), System.Globalization.NumberStyles.HexNumber);
            AlphaComponent = components.Length is 4
                ? byte.Parse(components[3].ToString(), System.Globalization.NumberStyles.HexNumber)
                : (byte)255;
        }
        else if (components.Length is 6 or 8)
        {
            RedComponent = byte.Parse(components[0..2], System.Globalization.NumberStyles.HexNumber);
            GreenComponent = byte.Parse(components[2..4], System.Globalization.NumberStyles.HexNumber);
            BlueComponent = byte.Parse(components[4..6], System.Globalization.NumberStyles.HexNumber);
            AlphaComponent = components.Length is 8
                ? byte.Parse(components[6..8], System.Globalization.NumberStyles.HexNumber)
                : (byte)255;
        }
    }

    private HexColorCode(byte red, byte green, byte blue, byte alpha)
        => (RedComponent, GreenComponent, BlueComponent, AlphaComponent) = (red, green, blue, alpha);

    public static implicit operator HexColorCode(string hexColorCode)
        => VALID_HEX_CRITERIA.IsMatch(hexColorCode)
            ? new HexColorCode(hexColorCode)
            : throw new InvalidHexColorCodeException(hexColorCode);

    public static implicit operator HexColorCode((byte red, byte green, byte blue, byte alpha) hexColorCode)
        => new(hexColorCode.red, hexColorCode.green, hexColorCode.blue, hexColorCode.alpha);

    public static explicit operator HexColorCode(RGB rgb)
        => $"#{rgb.RedComponent:X2}{rgb.GreenComponent:X2}{rgb.BlueComponent:X2}";

    public static explicit operator HexColorCode(RGBA rgba)
        => $"#{rgba.RedComponent:X2}{rgba.GreenComponent:X2}{rgba.BlueComponent:X2}{rgba.AlphaComponent:X2}";

    public override string ToString()
        => Value;
}
