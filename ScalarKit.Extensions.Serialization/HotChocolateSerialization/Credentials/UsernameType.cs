using HotChocolate.Language;

namespace ScalarKit.Extensions.Serialization;

public sealed class UsernameType : ScalarType
{
	public UsernameType()
		: base(nameof(Username))
		=> Description = "The `Username` scalar type represents a valid username given a criteria.";

	public override Type RuntimeType => typeof(string);

	public override bool IsInstanceOfType(IValueNode valueSyntax)
		=> valueSyntax is StringValueNode stringValueNode
		 && Username.TryFrom(stringValueNode.Value, out _);

	public override object? ParseLiteral(IValueNode valueSyntax)
		=> (Username)((StringValueNode)valueSyntax).Value;

	public override IValueNode ParseResult(object? resultValue)
		=> new StringValueNode((string)resultValue!);

	public override IValueNode ParseValue(object? runtimeValue)
		=> new StringValueNode((string)runtimeValue!);

	public override bool TrySerialize(object? runtimeValue, out object? resultValue)
	{
		if (runtimeValue is not string)
		{
			resultValue = null;

			return false;
		}

		try
		{
			resultValue = (Username)(string)runtimeValue;

			return true;
		}
		catch
		{
			resultValue = null;

			return false;
		}
	}

	public override bool TryDeserialize(object? resultValue, out object? runtimeValue)
	{
		if (resultValue is not string)
		{
			runtimeValue = null;

			return false;
		}

		try
		{
			runtimeValue = (Username)(string)resultValue;

			return true;
		}
		catch
		{
			runtimeValue = null;

			return false;
		}
	}
}
