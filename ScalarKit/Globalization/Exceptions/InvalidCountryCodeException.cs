namespace ScalarKit.Exceptions;

public sealed class InvalidCountryCodeException : Exception
{
    public InvalidCountryCodeException()
        : base("The country code is invalid.") { }

    public InvalidCountryCodeException(string countryCode)
        : base($"The country code is invalid: {countryCode}.") { }
}
