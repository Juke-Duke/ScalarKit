using System.Text.RegularExpressions;
using ScalarKit.Exceptions;

namespace ScalarKit;

public sealed record Username : IScalar<Username, string>
{
    private static readonly Regex DEFAULT_VALID_CRITERIA = new(@"^(?=.{1,21}$)(?=.*[a-zA-Z])");

    private const string DEFAULT_VALID_CRITERIA_DETAILS = """
        - Must be between 1 and 21 characters long.
        - Must contain at least one letter.
        """;

    public string Value { get; }

    private Username(string username)
        : this(username, DEFAULT_VALID_CRITERIA, DEFAULT_VALID_CRITERIA_DETAILS) { }

    public Username(string username, Regex citeria, string criteriaDetails)
        => Value = citeria.IsMatch(username)
            ? username
            : throw new InvalidUsernameException(username, criteriaDetails);

    public static implicit operator Username(string username)
        => new(username);
}
