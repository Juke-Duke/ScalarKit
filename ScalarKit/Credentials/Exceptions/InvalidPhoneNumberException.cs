namespace ScalarKit.Exceptions;

public sealed class InvalidPhoneNumberException : Exception
{
    public InvalidPhoneNumberException()
        : base("The provided phone number is invalid.") { }

    public InvalidPhoneNumberException(string phoneNumber)
        : base($"The provided phone number is invalid: {phoneNumber}.") { }
}
