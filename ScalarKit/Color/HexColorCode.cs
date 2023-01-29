using System.Text.RegularExpressions;
using ScalarKit.Exceptions;

namespace ScalarKit;

public readonly record struct HexColorCode : IScalar<HexColorCode, string>
{
    private static readonly Regex VALID_HEX_CRITERIA = new Regex(@"#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3,4}|[A-Fa-f0-9]{8})$");

    public string RedComponentHex { get; }

    public string GreenComponentHex { get; }

    public string BlueComponentHex { get; }

    public string AlphaComponentHex { get; }

    public string Value { get; }

    private HexColorCode(string hexColorCode)
    {
        Value = hexColorCode;

        if (Value.Length == 4)
        {
            RedComponentHex = Value[1].ToString();
            GreenComponentHex = Value[2].ToString();
            BlueComponentHex = Value[3].ToString();
            AlphaComponentHex = "";
        }
        else if (Value.Length == 5)
        {
            RedComponentHex = Value[1].ToString();
            GreenComponentHex = Value[2].ToString();
            BlueComponentHex = Value[3].ToString();
            AlphaComponentHex = Value[4].ToString();
        }
        else if (Value.Length == 7)
        {
            RedComponentHex = Value[1..3];
            GreenComponentHex = Value[3..5];
            BlueComponentHex = Value[5..7];
            AlphaComponentHex = "";
        }
        else
        {
            RedComponentHex = Value[1..3];
            GreenComponentHex = Value[3..5];
            BlueComponentHex = Value[5..7];
            AlphaComponentHex = Value[7..9];
        }
    }

    public HexColorCode(string redComponentHex, string greenComponentHex, string blueComponentHex, string alphaComponentHex = "")
        => (RedComponentHex, GreenComponentHex, BlueComponentHex, AlphaComponentHex, Value) = (redComponentHex, greenComponentHex, blueComponentHex, alphaComponentHex, $"#{redComponentHex}{greenComponentHex}{blueComponentHex}{alphaComponentHex}");

    public static implicit operator HexColorCode(string hexColorCode)
        => VALID_HEX_CRITERIA.IsMatch(hexColorCode)
            ? new HexColorCode(hexColorCode)
            : throw new InvalidHexColorCodeException(hexColorCode);

    public static explicit operator HexColorCode(RGB rgb)
        => $"#{rgb.RedComponent:X2}{rgb.GreenComponent:X2}{rgb.BlueComponent:X2}";

    // public static explicit operator HexColorCode(RGBA rgba)
    //     => $"#{rgba.RedComponent:X2}{rgba.GreenComponent:X2}{rgba.BlueComponent:X2}{rgba.AlphaComponent:X2}";

    public override string ToString()
        => $"#{RedComponentHex}{GreenComponentHex}{BlueComponentHex}{AlphaComponentHex}";
}
