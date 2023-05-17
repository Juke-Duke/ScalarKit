using System.Text.RegularExpressions;

namespace ScalarKit;

/// <summary>
///     Represents a valid username with given criteria.
/// </summary>
public sealed record Username : IScalar<Username, string>
{
	/// <summary>
	///     Gets the current criteria that a <see cref="Username" /> must meet. This can be changed at any time globally.
	/// </summary>
	public static Regex VALID_CRITERIA = new(@"^(?=.{1,21}$)(?=.*[a-zA-Z])");

	/// <summary>
	///     Gets the current criteria details that a <see cref="Username" /> must meet. This can be changed at any time
	///     globally.
	/// </summary>
	public static string VALID_CRITERIA_DETAILS = """
        - Must be between 1 and 21 characters long.
        - Must contain at least one letter.
        """;

	/// <summary>
	///     Initializes a new instance of <see cref="Username" />.
	/// </summary>
	/// <param name="username">The <see cref="string" /> to initialize this instance with.</param>
	public Username(string username)
		=> Value = username;

	/// <summary>
	///     Gets the value of this <see cref="Username" />.
	/// </summary>
	/// <returns>The value of this <see cref="Username" />.</returns>
	public string Value { get; }

	/// <summary>
	///     Implicitly converts a <see cref="string" /> to a <see cref="Username" /> using the current
	///     <see cref="VALID_CRITERIA" />.
	/// </summary>
	/// <param name="username">The <see cref="string" /> to convert.</param>
	public static implicit operator Username(string username)
		=> VALID_CRITERIA.IsMatch(username)
			? new Username(username)
			: throw new FormatException(
				$"{nameof(Username)} must meet the following criteria:\n{VALID_CRITERIA_DETAILS}"
			);

	/// <summary>
	///     Implicitly converts a <see cref="Username" /> to a <see cref="string" /> using the current
	///     <see cref="VALID_CRITERIA" />.
	/// </summary>
	/// <param name="string">The <see cref="string" /> to convert.</param>
	/// <param name="username">The <see cref="Username" /> that was converted.</param>
	/// <returns>
	///     <see langword="true" /> if the provided <see cref="string" /> is a valid <see cref="Username" />; otherwise,
	///     <see langword="false" />.
	/// </returns>
	public static bool TryFrom(string @string, out Username username)
	{
		try
		{
			username = @string;

			return true;
		}
		catch
		{
			username = default!;

			return false;
		}
	}

	/// <summary>
	///     Returns a <see cref="string" /> that represents the current <see cref="Username" />.
	/// </summary>
	/// <returns>A <see cref="string" /> that represents the current <see cref="Username" />.</returns>
	public override string ToString()
		=> Value;
}
