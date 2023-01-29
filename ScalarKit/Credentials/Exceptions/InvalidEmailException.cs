namespace ScalarKit.Exceptions;

public sealed class InvalidEmailException : Exception
{
    public InvalidEmailException()
        : base("The email is invalid.") { }

    public InvalidEmailException(string email)
        : base($"The email is invalid: {email}.") { }
}
