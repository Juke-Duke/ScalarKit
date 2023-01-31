using System.Text.RegularExpressions;
using ScalarKit.Exceptions;

namespace ScalarKit;

public sealed record Username : IScalar<Username, string>
{
    public static Regex VALID_CRITERIA = new(@"^(?=.{1,21}$)(?=.*[a-zA-Z])");

    public static string VALID_CRITERIA_DETAILS = """
        - Must be between 1 and 21 characters long.
        - Must contain at least one letter.
        """;

    public string Value { get; }

    public Username(string username)
        => Value = username;

    public static implicit operator Username(string username)
        => VALID_CRITERIA.IsMatch(username)
            ? new Username(username)
            : throw new InvalidUsernameException(username, VALID_CRITERIA_DETAILS);
}
