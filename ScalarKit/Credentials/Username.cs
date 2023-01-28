using System.Text.RegularExpressions;
using ScalarKit.ErrorHandling;
using ScalarKit.Exceptions;

namespace ScalarKit;

public sealed record Username : IProneScalar<Username, string>
{
    private static readonly Regex DEFAULT_VALID_CRITERIA = new(@"^(?=.{1,21}$)(?=.*[a-zA-Z])");

    private const string DEFAULT_VALID_CRITERIA_DETAILS = """
        - Must be between 1 and 21 characters long.
        - Must contain at least one letter.
        """;

    public string Value { get; }

    private Username(string username)
        => Value = username;

    public static implicit operator Username(string username)
    {
        var inspectedUsername = Inspect(username);

        if (inspectedUsername.IsFaulty)
            inspectedUsername.ThrowFirstErrorIfFaulty();

        return inspectedUsername.Value;
    }

    public static ErrorProne<Username> Inspect(string username)
        => Inspect(username, DEFAULT_VALID_CRITERIA, DEFAULT_VALID_CRITERIA_DETAILS);

    public static ErrorProne<Username> Inspect(string username, Regex citeria, string criteriaDetails)
        => citeria.IsMatch(username)
            ? new Username(username)
            : new InvalidUsernameException(username, criteriaDetails);
}
