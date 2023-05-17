using System.Net.Mail;
using HotChocolate.Language;

namespace ScalarKit.Extensions.Serialization;

public sealed class EmailAddressType : StringType
{
	public EmailAddressType()
		: base(nameof(EmailAddress), "The `EmailAddress` scalar type represents a valid email address.") { }

	protected override bool IsInstanceOfType(string runtimeValue)
		=> EmailAddress.TryFrom(runtimeValue, out _);

	protected override bool IsInstanceOfType(StringValueNode valueSyntax)
		=> EmailAddress.TryFrom(valueSyntax.Value, out _);

	protected override string ParseLiteral(StringValueNode valueSyntax)
		=> valueSyntax.Value;

	protected override StringValueNode ParseValue(string runtimeValue)
		=> new(runtimeValue);

	public override IValueNode ParseResult(object? resultValue)
		=> ParseValue(resultValue);

	public override bool TrySerialize(object? runtimeValue, out object? resultValue)
	{
		if (runtimeValue is not string)
		{
			resultValue = null;
			return false;
		}

		if (EmailAddress.TryFrom((string)runtimeValue, out _))
		{
			resultValue = runtimeValue;
			return true;
		}

		resultValue = null;
		return false;
	}

	public override bool TryDeserialize(object? resultValue, out object? runtimeValue)
	{
		if (resultValue is not string)
		{
			runtimeValue = null;
			return false;
		}

		if (EmailAddress.TryFrom((string)resultValue, out _))
		{
			runtimeValue = resultValue;
			return true;
		}

		runtimeValue = null;
		return false;
	}
}
