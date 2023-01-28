namespace ScalarKit.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException()
        : base("The provided password is invalid.") { }

    public InvalidPasswordException(string criteriaDetails)
        : base($"The provided password is invalid.\n{criteriaDetails}") { }
}
