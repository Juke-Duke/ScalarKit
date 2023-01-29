using System.Text.RegularExpressions;
using ScalarKit.Exceptions;

namespace ScalarKit;

public readonly record struct HexColorCode : IScalar<HexColorCode, string>
{
    private static readonly Regex VALID_HEX_CRITERIA = new Regex(@"#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3,4}|[A-Fa-f0-9]{8})$");

    public string RedHexComponent { get; }

    public string GreenHexComponent { get; }

    public string BlueHexComponent { get; }

    public string AlphaHexComponent { get; }

    public string Value { get; }

    private HexColorCode(string hexColorCode)
    {
        Value = hexColorCode;

        if (Value.Length == 4)
        {
            RedHexComponent = Value[1].ToString();
            GreenHexComponent = Value[2].ToString();
            BlueHexComponent = Value[3].ToString();
            AlphaHexComponent = "";
        }
        else if (Value.Length == 5)
        {
            RedHexComponent = Value[1].ToString();
            GreenHexComponent = Value[2].ToString();
            BlueHexComponent = Value[3].ToString();
            AlphaHexComponent = Value[4].ToString();
        }
        else if (Value.Length == 7)
        {
            RedHexComponent = Value[1..3];
            GreenHexComponent = Value[3..5];
            BlueHexComponent = Value[5..7];
            AlphaHexComponent = "";
        }
        else
        {
            RedHexComponent = Value[1..3];
            GreenHexComponent = Value[3..5];
            BlueHexComponent = Value[5..7];
            AlphaHexComponent = Value[7..9];
        }
    }

    public HexColorCode(string redComponentHex, string greenComponentHex, string blueComponentHex, string alphaComponentHex = "")
        => (RedHexComponent, GreenHexComponent, BlueHexComponent, AlphaHexComponent, Value) = (redComponentHex, greenComponentHex, blueComponentHex, alphaComponentHex, $"#{redComponentHex}{greenComponentHex}{blueComponentHex}{alphaComponentHex}");

    public static implicit operator HexColorCode(string hexColorCode)
        => VALID_HEX_CRITERIA.IsMatch(hexColorCode)
            ? new HexColorCode(hexColorCode)
            : throw new InvalidHexColorCodeException(hexColorCode);

    public static explicit operator HexColorCode(RGB rgb)
        => $"#{rgb.RedComponent:X2}{rgb.GreenComponent:X2}{rgb.BlueComponent:X2}";

    public static explicit operator HexColorCode(RGBA rgba)
        => $"#{rgba.RedComponent:X2}{rgba.GreenComponent:X2}{rgba.BlueComponent:X2}{rgba.AlphaComponent:X2}";

    public override string ToString()
        => $"#{RedHexComponent}{GreenHexComponent}{BlueHexComponent}{AlphaHexComponent}";
}
