using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ScalarKit.Exceptions;

namespace ScalarKit;

public sealed record Password : IScalar<Password, string>
{
    /// <summary>
    /// The default regex to validate a <see cref="Password"/>.<br/>
    /// - Must contain at least one lowercase letter. <br/>
    /// - Must contain at least one uppercase letter. <br/>
    /// - Must contain at least one digit. <br/>
    /// - Must contain at least one non-alphanumeric character. <br/>
    /// - Must be at least 6 characters long.
    /// </summary>
    /// <value>The string containing the default regex to validate a <see cref="Password"/>.</value>
    private static readonly Regex DEFAULT_VALID_CRITERIA = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{6,}$");

    private const string DEFAULT_VALID_CRITERIA_DETAILS = """
        - Must contain at least one lowercase letter.
        - Must contain at least one uppercase letter.
        - Must contain at least one digit.
        - Must contain at least one non-alphanumeric character.
        - Must be at least 6 characters long.
        """;

    public string Value { get; private set; }

    public bool IsEncrypted { get; private set; }

    private Password(string password)
        => Value = password;

    public static implicit operator Password(string password)
        => new(password);

    public static Password Encrypt(Password password)
        => Encrypt(password, DEFAULT_VALID_CRITERIA, DEFAULT_VALID_CRITERIA_DETAILS);

    public static Password Encrypt(Password password, Regex criteria, string criteriaDetails)
    {
        if (password.IsEncrypted)
            return password;

        if (!(criteria ?? DEFAULT_VALID_CRITERIA).IsMatch(password.Value))
            throw new InvalidPasswordException(criteriaDetails ?? DEFAULT_VALID_CRITERIA_DETAILS);

        const int SALT_KEY = 64;
        const int ITERATIONS = 1024;

        var salt = RandomNumberGenerator.GetBytes(SALT_KEY);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password: Encoding.UTF8.GetBytes(password.Value),
            salt: salt,
            iterations: ITERATIONS,
            hashAlgorithm: HashAlgorithmName.SHA512,
            outputLength: SALT_KEY
        );

        password.Value = Convert.ToHexString(hash);
        password.IsEncrypted = true;

        return password;
    }
}
