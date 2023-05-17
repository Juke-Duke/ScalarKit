namespace ScalarKit;

public interface IScalar<TSelf, TPrimitive>
	where TSelf : notnull, IScalar<TSelf, TPrimitive>
	where TPrimitive : notnull
{
	TPrimitive Value { get; }

	static abstract implicit operator TSelf(TPrimitive primitive);

	static abstract bool TryFrom(TPrimitive primitive, out TSelf? scalar);

	string ToString();
}
