namespace ScalarKit.Exceptions;

public sealed class InvalidEmailAddressException : Exception
{
    public InvalidEmailAddressException()
        : base("The email is invalid.") { }

    public InvalidEmailAddressException(string email)
        : base($"The email is invalid: {email}.") { }
}
