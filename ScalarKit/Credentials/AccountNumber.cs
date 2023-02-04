using System.Text.RegularExpressions;

namespace ScalarKit.Credentials;

public readonly record struct AccountNumber : IScalar<AccountNumber, string>
{
    private static readonly Regex VALID_CRITERIA = new Regex(@"^([a-zA-Z0-9]){5,17}$");

    public string Value { get; }

    private AccountNumber(string accountNumber)
        => Value = accountNumber;

    public static implicit operator AccountNumber(string accountNumber)
        => VALID_CRITERIA.IsMatch(accountNumber)
            ? new AccountNumber(accountNumber)
            : throw new FormatException($"{nameof(AccountNumber)} must be between 5 and 17 characters long and can only contain letters and numbers.");
}
