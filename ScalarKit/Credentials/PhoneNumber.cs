using System.Text.RegularExpressions;

namespace ScalarKit;

/// <summary>
///     Represents a valid phone number.
/// </summary>
public readonly record struct PhoneNumber : IScalar<PhoneNumber, string>, IFormattable
{
	/// <summary>
	///     Gets the criteria that a <see cref="PhoneNumber" /> must meet.
	/// </summary>
	/// <returns></returns>
	private static readonly Regex VALID_CRITERIA =
		new(@"^(\+\d{1,3}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$");

	/// <summary>
	///     Initializes a new instance of <see cref="PhoneNumber" />.
	/// </summary>
	/// <param name="digits">The <see cref="char" /> array representing the digits of this <see cref="PhoneNumber" />.</param>
	private PhoneNumber(char[] digits)
	{
		int countryCodeLength = digits.Length - 10;
		CountryCode = countryCodeLength > 0 ? int.Parse(digits[..countryCodeLength]) : 1;
		AreaCode = int.Parse(digits[countryCodeLength..(countryCodeLength + 3)]);
		ExchangeCode = int.Parse(digits[(countryCodeLength + 3)..(countryCodeLength + 6)]);
		LineNumber = int.Parse(digits[(countryCodeLength + 6)..]);
	}

	/// <summary>
	///     Gets the country code component of this <see cref="PhoneNumber" />.
	/// </summary>
	/// <returns>The country code component of this <see cref="PhoneNumber" />.</returns>
	public int CountryCode { get; }

	/// <summary>
	///     Gets the area code component of this <see cref="PhoneNumber" />.
	/// </summary>
	/// <returns>The area code component of this <see cref="PhoneNumber" />.</returns>
	public int AreaCode { get; }

	/// <summary>
	///     Gets the exchange code component of this <see cref="PhoneNumber" />.
	/// </summary>
	/// <returns>The exchange code component of this <see cref="PhoneNumber" />.</returns>
	public int ExchangeCode { get; }

	/// <summary>
	///     Gets the line number component of this <see cref="PhoneNumber" />.
	/// </summary>
	/// <returns>The line number component of this <see cref="PhoneNumber" />.</returns>
	public int LineNumber { get; }

	/// <summary>
	///     Returns a <see cref="string" /> that represents the current <see cref="PhoneNumber" /> in the specified format.
	/// </summary>
	/// <param name="format">The <see cref="PhoneNumber" /> format to use.</param>
	/// <param name="formatProvider">The provider is ignored.</param>
	/// <returns>A <see cref="string" /> that represents the current <see cref="PhoneNumber" /> in the specified format.</returns>
	public string ToString(string? format, IFormatProvider? formatProvider = null)
	{
		if (string.IsNullOrEmpty(format))
			return ToString();

		return format switch
		{
			"E164" => $"+{CountryCode}{AreaCode}{ExchangeCode}{LineNumber}",
			"L"    => $"{ExchangeCode}{LineNumber}",
			"D"    => $"({AreaCode}) {ExchangeCode}-{LineNumber}",
			"I"    => $"+{CountryCode}-{AreaCode}-{ExchangeCode}-{LineNumber}",
			"E"    => $"+{CountryCode} ({AreaCode}) {ExchangeCode}-{LineNumber}",
			_      => throw new FormatException($"The {format} format string is not supported.")
		};
	}

	/// <summary>
	///     Gets the full number of this <see cref="PhoneNumber" /> in E.164 format.
	/// </summary>
	/// <returns>The full number of this <see cref="PhoneNumber" /> in E.164 format.</returns>
	public string Value => $"+{CountryCode}{AreaCode}{ExchangeCode}{LineNumber}";

	/// <summary>
	///     Implicitly converts a <see cref="string" /> to a <see cref="PhoneNumber" />.
	/// </summary>
	/// <param name="phoneNumber">The <see cref="string" /> to convert.</param>
	public static implicit operator PhoneNumber(string phoneNumber)
		=> VALID_CRITERIA.IsMatch(phoneNumber)
			? new PhoneNumber(phoneNumber.Where(x => char.IsDigit(x)).ToArray())
			: throw new FormatException($"{nameof(PhoneNumber)} value must be a valid phone number.");

	/// <summary>
	///     Attempts to convert the specified <see cref="string" /> to a <see cref="PhoneNumber" />.
	/// </summary>
	/// <param name="primitive">The <see cref="string" /> to convert.</param>
	/// <param name="scalar">The <see cref="PhoneNumber" /> that was converted.</param>
	/// <returns>
	///     <see langword="true" /> if the provided is a valid <see cref="PhoneNumber" />; otherwise,
	///     <see langword="false" />.
	/// </returns>
	public static bool TryFrom(string primitive, out PhoneNumber scalar)
	{
		try
		{
			scalar = primitive;

			return true;
		}
		catch
		{
			scalar = default;

			return false;
		}
	}

	/// <summary>
	///     Returns a <see cref="string" /> that represents the current <see cref="PhoneNumber" /> in E.164 format.
	/// </summary>
	/// <returns>A <see cref="string" /> that represents the current <see cref="PhoneNumber" /> in E.164 format.</returns>
	public override string ToString()
		=> ToString("E164");
}
