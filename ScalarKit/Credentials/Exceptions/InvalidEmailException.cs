namespace ScalarKit.Exceptions;

public sealed class InvalidEmailException : Exception
{
    public InvalidEmailException()
        : base("The provided email is invalid.") { }

    public InvalidEmailException(string email)
        : base($"The provided email is invalid: {email}.") { }
}
