using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ScalarKit;

public sealed record Password : IScalar<Password, string>
{
    public static Regex VALID_CRITERIA = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{6,}$");

    public static string VALID_CRITERIA_DETAILS = """
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
        => VALID_CRITERIA.IsMatch(password)
            ? new Password(password)
            : throw new FormatException($"{nameof(Password)} must meet the following criteria:{Environment.NewLine}{VALID_CRITERIA_DETAILS}");

    public static Password Encrypt(Password password)
    {
        if (password.IsEncrypted)
            return password;

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
