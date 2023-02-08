using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ScalarKit;

/// <summary>
///     Represents a valid password with given criteria.
/// </summary>
public sealed record Password : IScalar<Password, string>
{
	/// <summary>
	///     Gets the current criteria that a <see cref="Password" /> must meet. This can be changed at any time globally.
	/// </summary>
	public static Regex VALID_CRITERIA = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{6,}$");

	/// <summary>
	///     Gets the current criteria details that a <see cref="Password" /> must meet. This can be changed at any time
	///     globally.
	/// </summary>
	public static string VALID_CRITERIA_DETAILS = """
        - Must contain at least one lowercase letter.
        - Must contain at least one uppercase letter.
        - Must contain at least one digit.
        - Must contain at least one non-alphanumeric character.
        - Must be at least 6 characters long.
        """;

	/// <summary>
	///     Initializes a new instance of <see cref="Password" />.
	/// </summary>
	/// <param name="password">The <see cref="string" /> to initialize this instance with.</param>
	private Password(string password)
		=> Value = password;

	/// <summary>
	///     Gets an indication whether the current <see cref="Password" /> is encrypted.
	/// </summary>
	/// <returns>
	///     <see langword="true" /> if the current <see cref="Password" /> is encrypted; otherwise,
	///     <see langword="false" />.
	/// </returns>
	public bool IsEncrypted { get; private set; }

	/// <summary>
	///     Gets the value of this <see cref="Password" />.
	/// </summary>
	/// <returns>The value of this <see cref="Password" />.</returns>
	public string Value { get; private set; }

	/// <summary>
	///     Implicitly converts a <see cref="string" /> to a <see cref="Password" /> using the current
	///     <see cref="VALID_CRITERIA" />.
	/// </summary>
	/// <param name="password">The <see cref="string" /> to convert.</param>
	public static implicit operator Password(string password)
		=> VALID_CRITERIA.IsMatch(password)
			? new Password(password)
			: throw new FormatException(
				$"{nameof(Password)} must meet the following criteria:{Environment.NewLine}{VALID_CRITERIA_DETAILS}"
			);

	/// <summary>
	///     Attempts to convert the specified <see cref="string" /> to a <see cref="Password" /> using the current
	///     <see cref="VALID_CRITERIA" />.
	/// </summary>
	/// <param name="string">The <see cref="string" /> to convert.</param>
	/// <param name="password">The <see cref="Password" /> that was converted.</param>
	/// <returns>
	///     <see langword="true" /> if the provided <see cref="string" /> is a valid <see cref="Password" />; otherwise,
	///     <see langword="false" />.
	/// </returns>
	public static bool TryFrom(string @string, out Password password)
	{
		try
		{
			password = @string;

			return true;
		}
		catch
		{
			password = default!;

			return false;
		}
	}

	/// <summary>
	///     Returns a <see cref="string" /> that represents the current <see cref="Password" />.
	/// </summary>
	/// <returns>A <see cref="string" /> that represents the current <see cref="Password" />.</returns>
	public override string ToString()
		=> Value;

	/// <summary>
	///     Encrypts the current <see cref="Password" />.
	/// </summary>
	/// <param name="password">The <see cref="Password" /> to encrypt.</param>
	/// <returns></returns>
	public static Password Encrypt(Password password)
	{
		if (password.IsEncrypted)
			return password;

		const int SALT_KEY = 64;
		const int ITERATIONS = 1024;

		byte[] salt = RandomNumberGenerator.GetBytes(SALT_KEY);

		byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
			Encoding.UTF8.GetBytes(password.Value),
			salt,
			ITERATIONS,
			HashAlgorithmName.SHA512,
			SALT_KEY
		);

		password.Value = Convert.ToHexString(hash);
		password.IsEncrypted = true;

		return password;
	}
}
