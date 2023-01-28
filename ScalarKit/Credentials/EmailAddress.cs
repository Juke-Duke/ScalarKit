using System.Net.Mail;
using ScalarKit.ErrorHandling;
using ScalarKit.Exceptions;

namespace ScalarKit;

public sealed record EmailAddress : IProneScalar<EmailAddress, string>
{
    public string Username { get; }

    public string Domain { get; }

    public string Value => $"{Username}@{Domain}";

    private EmailAddress(string emailAddress)
    {
        var components = emailAddress.Split('@');

        Username = components[0];
        Domain = components[1];
    }

    public static implicit operator EmailAddress(string emailAddress)
    {
        var inspectedEmail = Inspect(emailAddress);

        inspectedEmail.ThrowFirstErrorIfFaulty();

        return inspectedEmail.Value;
    }

    public static implicit operator string(EmailAddress emailAddress)
        => emailAddress.Value;

    public static ErrorProne<EmailAddress> Inspect(string emailAddress)
    {
        if (!MailAddress.TryCreate(emailAddress, out _))
            return new InvalidEmailException(emailAddress);

        return new EmailAddress(emailAddress);
    }
}
