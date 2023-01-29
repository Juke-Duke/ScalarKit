using System.Text.RegularExpressions;
using ScalarKit.Exceptions;

namespace ScalarKit;

public readonly record struct PhoneNumber : IScalar<PhoneNumber, string>, IFormattable
{
    private static readonly Regex VALID_CRITERIA
        = new Regex(@"^(\+\d{1,3}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$");

    public int CountryCode { get; }

    public int AreaCode { get; }

    public int ExchangeCode { get; }

    public int LineNumber { get; }

    public string Value => ToString();

    private PhoneNumber(char[] digits)
    {
        var countryCodeLength = digits.Length - 10;
        CountryCode = countryCodeLength > 0 ? int.Parse(digits[..countryCodeLength]) : 1;
        AreaCode = int.Parse(digits[countryCodeLength..(countryCodeLength + 3)]);
        ExchangeCode = int.Parse(digits[(countryCodeLength + 3)..(countryCodeLength + 6)]);
        LineNumber = int.Parse(digits[(countryCodeLength + 6)..]);
    }

    public static implicit operator PhoneNumber(string phoneNumber)
        => VALID_CRITERIA.IsMatch(phoneNumber)
            ? new PhoneNumber(phoneNumber.Where(x => char.IsDigit(x)).ToArray())
            : throw new InvalidPhoneNumberException(phoneNumber);

    public override string ToString()
        => ToString("E164", null);

    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        if (string.IsNullOrEmpty(format))
            return ToString();

        return format switch
        {
            "E164" => $"+{CountryCode}{AreaCode}{ExchangeCode}{LineNumber}",
            "L" => $"{ExchangeCode}{LineNumber}",
            "D" => $"({AreaCode}) {ExchangeCode}-{LineNumber}",
            "I" => $"+{CountryCode}-{AreaCode}-{ExchangeCode}-{LineNumber}",
            "E" => $"+{CountryCode} ({AreaCode}) {ExchangeCode}-{LineNumber}",
            _ => throw new FormatException($"The {format} format string is not supported.")
        };
    }
}
