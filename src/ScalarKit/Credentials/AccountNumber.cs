using System.Text.RegularExpressions;

namespace ScalarKit.Credentials;

/// <summary>
///     Represents a valid banking account number.
/// </summary>
public readonly record struct AccountNumber : IScalar<AccountNumber, string>
{
	/// <summary>
	///     Gets the criteria that a <see cref="AccountNumber" /> must meet.
	/// </summary>
	/// <returns>The criteria that a <see cref="AccountNumber" /> must meet.</returns>
	private static readonly Regex VALID_CRITERIA = new(@"^([a-zA-Z0-9]){5,17}$");

	/// <summary>
	///     Gets the criteria that a <see cref="AccountNumber" /> must meet.
	/// </summary>
	/// <param name="accountNumber"></param>
	private AccountNumber(string accountNumber)
		=> Value = accountNumber;

	/// <summary>
	///     Gets the value of this <see cref="AccountNumber" />.
	/// </summary>
	/// <returns>The value of this <see cref="AccountNumber" />.</returns>
	public string Value { get; }

	/// <summary>
	///     Implicitly converts a <see cref="string" /> to an <see cref="AccountNumber" />.
	/// </summary>
	/// <param name="accountNumber">The <see cref="string" /> to convert.</param>
	public static implicit operator AccountNumber(string accountNumber)
		=> VALID_CRITERIA.IsMatch(accountNumber)
			? new AccountNumber(accountNumber)
			: throw new FormatException(
				$"{nameof(AccountNumber)} must be between 5 and 17 characters long and can only contain letters and numbers."
			);

	/// <summary>
	///     Implicitly converts an <see cref="AccountNumber" /> to a <see cref="string" />.
	/// </summary>
	/// <param name="primitive">The <see cref="string" /> to convert.</param>
	/// <param name="scalar">The <see cref="AccountNumber" /> that was converted.</param>
	/// <returns>
	///     <see langword="true" /> if the provided <see cref="string" /> is a valid <see cref="AccountNumber" />;
	///     otherwise, <see langword="false" />.
	/// </returns>
	public static bool TryFrom(string primitive, out AccountNumber scalar)
	{
		try
		{
			scalar = primitive;

			return true;
		}
		catch
		{
			scalar = default;

			return false;
		}
	}

	/// <summary>
	///     Returns a <see cref="string" /> that represents the current <see cref="AccountNumber" />.
	/// </summary>
	/// <returns>A <see cref="string" /> that represents the current <see cref="AccountNumber" />.</returns>
	public override string ToString()
		=> Value;
}
