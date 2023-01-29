namespace ScalarKit.Exceptions;

public sealed class InvalidCurrencyException : Exception
{
    public InvalidCurrencyException()
        : base("The currency is invalid.") { }

    public InvalidCurrencyException(string currency)
        : base($"The currency is invalid: {currency}.") { }
}
