using System.Globalization;
using System.Text.RegularExpressions;

namespace ScalarKit;

public readonly record struct HexColorCode : IScalar<HexColorCode, string>
{
	private static readonly Regex VALID_HEX_CRITERIA = new(@"#([A-Fa-f0-9]{3,4}|[A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$");

	public static bool Lowercase = false;

	private HexColorCode(string hexColorCode)
	{
		string components = hexColorCode[1..];

		if (components.Length is 3 or 4)
		{
			RedComponent = byte.Parse(components[0].ToString(), NumberStyles.HexNumber);
			GreenComponent = byte.Parse(components[1].ToString(), NumberStyles.HexNumber);
			BlueComponent = byte.Parse(components[2].ToString(), NumberStyles.HexNumber);
			Alpha = components.Length is 4
				? byte.Parse(components[3].ToString(), NumberStyles.HexNumber)
				: (byte)255;
		}
		else if (components.Length is 6 or 8)
		{
			RedComponent = byte.Parse(components[..2], NumberStyles.HexNumber);
			GreenComponent = byte.Parse(components[2..4], NumberStyles.HexNumber);
			BlueComponent = byte.Parse(components[4..6], NumberStyles.HexNumber);
			Alpha = components.Length is 8
				? byte.Parse(components[6..8], NumberStyles.HexNumber)
				: (byte)255;
		}
	}

	private HexColorCode(byte red, byte green, byte blue, byte alpha)
		=> (RedComponent, GreenComponent, BlueComponent, Alpha) = (red, green, blue, alpha);

	private static string Format => Lowercase ? "x2" : "X2";

	public byte RedComponent { get; }

	public byte GreenComponent { get; }

	public byte BlueComponent { get; }

	public byte Alpha { get; }

	public string Value
		=> $"#{RedComponent.ToString(Format)}{GreenComponent.ToString(Format)}{BlueComponent.ToString(Format)}{Alpha.ToString(Format)}";

	public static implicit operator HexColorCode(string hexColorCode)
		=> VALID_HEX_CRITERIA.IsMatch(hexColorCode)
			? new HexColorCode(hexColorCode)
			: throw new FormatException(
				$"{nameof(HexColorCode)} must be in the format of one of '#RGB', '#RGBA', '#RRGGBB', or '#RRGGBBAA'"
			);

	public override string ToString()
		=> Value;

	public static bool TryFrom(string primitive, out HexColorCode scalar) => throw new NotImplementedException();

	public static implicit operator HexColorCode((byte red, byte green, byte blue, byte alpha) hexColorCode)
		=> new(hexColorCode.red, hexColorCode.green, hexColorCode.blue, hexColorCode.alpha);

	public static explicit operator HexColorCode(RGB rgb)
		=> $"#{rgb.RedComponent:X2}{rgb.GreenComponent:X2}{rgb.BlueComponent:X2}";

	public static explicit operator HexColorCode(RGBA rgba)
		=> $"#{rgba.RedComponent:X2}{rgba.GreenComponent:X2}{rgba.BlueComponent:X2}{rgba.Alpha:X2}";

	public static explicit operator HexColorCode(HSL hsl)
		=> (HexColorCode)(RGB)hsl;

	public static explicit operator HexColorCode(HSLA hsla)
		=> (HexColorCode)(RGBA)hsla;

	public void Deconstruct(out byte red, out byte green, out byte blue, out byte alpha)
		=> (red, green, blue, alpha) = (RedComponent, GreenComponent, BlueComponent, Alpha);
}
