using System.Net.Mail;

namespace ScalarKit;

public readonly record struct EmailAddress : IScalar<EmailAddress, string>
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
        => MailAddress.TryCreate(emailAddress, out _)
            ? new EmailAddress(emailAddress)
            : throw new FormatException($"{nameof(EmailAddress)} must be a valid email address.");

    public static implicit operator string(EmailAddress emailAddress)
        => emailAddress.Value;
}
