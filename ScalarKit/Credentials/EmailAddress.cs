using System.Text.RegularExpressions;

namespace ScalarKit;

/// <summary>
///     Represents a valid email address.
/// </summary>
public readonly record struct EmailAddress : IScalar<EmailAddress, string>
{
	private static readonly Regex VALID_CRITERIA = new(@"^(?!\.)(?!.*\.\.)[a-zA-Z0-9!#$%&'*+-/=?^_`{|}~\.]{0,63}[^\.]@(?!\-)[a-zA-Z0-9-]+[^\-]\.?[a-zA-Z]{2,255}$");

	/// <summary>
	///     Gets the username component of this <see cref="EmailAddress" />.
	/// </summary>
	/// <returns>The username component of this <see cref="EmailAddress" />.</returns>
	public string Local { get; }

	/// <summary>
	///     Gets the domain component of this <see cref="EmailAddress" />.
	/// </summary>
	/// <returns>The domain component of this <see cref="EmailAddress" />.</returns>
	public string Domain { get; }

	/// <summary>
	///     Gets the full address of this <see cref="EmailAddress" />.
	/// </summary>
	/// <returns>The full address of this <see cref="EmailAddress" />.</returns>
	public string Value => $"{Local}@{Domain}";

	/// <summary>
	///     Initializes a new instance of <see cref="EmailAddress" />.
	/// </summary>
	/// <param name="emailAddress">The <see cref="string" /> to initialize this instance with.</param>
	private EmailAddress(string emailAddress)
	{
		string[] components = emailAddress.Split('@');

		Local = components[0];
		Domain = components[1];
	}

	/// <summary>
	///     Implicitly converts a <see cref="string" /> to an <see cref="EmailAddress" />.
	/// </summary>
	/// <param name="emailAddress">The <see cref="string" /> to convert.</param>
	public static implicit operator EmailAddress(string emailAddress)
		=> VALID_CRITERIA.IsMatch(emailAddress)
			? new EmailAddress(emailAddress)
			: throw new FormatException($"{nameof(EmailAddress)} is invalid: {emailAddress}");

	/// <summary>
	///     Attempts to convert the specified <see cref="string" /> to an <see cref="EmailAddress" />.
	/// </summary>
	/// <param name="string">The <see cref="string" /> to convert.</param>
	/// <param name="emailAddress">The <see cref="EmailAddress" /> that was converted.</param>
	/// <returns>
	///     <see langword="true" /> if the provided <see cref="string" /> is a valid <see cref="EmailAddress" />;
	///     otherwise, <see langword="false" />.
	/// </returns>
	public static bool TryFrom(string @string, out EmailAddress emailAddress)
	{
		try { emailAddress = @string; return true; }
		catch { emailAddress = default; return false; }
	}

	/// <summary>
	///     Returns the <see cref="Value" /> containing the full address of this <see cref="EmailAddress" />.
	/// </summary>
	/// <returns>The <see cref="Value" /> of this <see cref="EmailAddress" />.</returns>
	public override string ToString()
		=> Value;
}
