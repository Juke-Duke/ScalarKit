namespace ScalarKit.Exceptions;

public sealed class InvalidAccountNumberException : Exception
{
    public InvalidAccountNumberException()
        : base("The account number is invalid.") { }

    public InvalidAccountNumberException(string accountNumber)
        : base($"The account number is invalid: {accountNumber}.") { }
}
