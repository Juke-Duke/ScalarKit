namespace ScalarKit.Exceptions;

public sealed class InvalidUsernameException : Exception
{
    public InvalidUsernameException()
        : base("The provided username is invalid.") { }

    public InvalidUsernameException(string username)
        : base($"The provided username is invalid: {username}.") { }

    public InvalidUsernameException(string username, string criteriaDetails)
        : base($"The provided username is invalid: {username}.\n{criteriaDetails}") { }
}
