using System.Text.RegularExpressions;

namespace ScalarKit;

public readonly record struct HSLA : IScalar<HSLA, string>
{
	private static readonly Regex VALID_CRITERIA = new(
		@"^hsla\(\s*(-?\d+|-?\d*.\d+)\s*,\s*(-?\d+|-?\d*.\d+)%\s*,\s*(-?\d+|-?\d*.\d+)%\s*,\s*(-?\d+|-?\d*.\d+)\s*\)$"
	);

	private HSLA(string hsla)
	{
		string[] components = hsla.Split('(', ')', ',');

		Hue = double.Parse(components[1]);

		Saturation = (Percentage)components[2];

		Luminance = (Percentage)components[3];

		Alpha = (Percentage)components[4];
	}

	private HSLA(Degree hue, Percentage saturation, Percentage lightness, Percentage alpha)
		=> (Hue, Saturation, Luminance, Alpha) = (hue, saturation, lightness, alpha);

	public Degree Hue { get; }

	public Percentage Saturation { get; }

	public Percentage Luminance { get; }

	public Percentage Alpha { get; }

	public string Value => $"hsla({Hue.ToString().TrimEnd('°')}, {Saturation}, {Luminance}, {Alpha})";

	public static implicit operator HSLA(string hsla)
		=> VALID_CRITERIA.IsMatch(hsla)
			? new HSLA(hsla)
			: throw new FormatException($"{nameof(HSLA)} value must be in the format 'hsla(000, 00%, 00%, 00%)'.");

	public override string ToString()
		=> Value;

	public static bool TryFrom(string primitive, out HSLA scalar) => throw new NotImplementedException();

	public static implicit operator HSLA(
		(Degree hue, Percentage saturation, Percentage lightness, Percentage alpha) hsla
	)
		=> new(hsla.hue, hsla.saturation, hsla.lightness, hsla.alpha);

	public static explicit operator HSLA(HexColorCode hexColorCode)
		=> (HSLA)(RGBA)hexColorCode;

	public static explicit operator HSLA(RGB rgb)
		=> (HSLA)(HSL)rgb;

	public static explicit operator HSLA(RGBA rgba)
	{
		(Degree hue, Percentage saturation, Percentage luminance) = (HSL)rgba;

		return new HSLA(hue, saturation, luminance, rgba.Alpha);
	}

	public static explicit operator HSLA(HSL hsl)
		=> new(hsl.Hue, hsl.Saturation, hsl.Luminance, (Percentage)"100%");

	public void Deconstruct(out Degree hue, out Percentage saturation, out Percentage lightness, out Percentage alpha)
		=> (hue, saturation, lightness, alpha) = (Hue, Saturation, Luminance, Alpha);
}
