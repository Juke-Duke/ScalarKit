namespace ScalarKit.Exceptions;

public sealed class FaultyValueException : Exception
{
	public FaultyValueException() { }

	public FaultyValueException(object errorProne)
		: base($"The error prone {errorProne.GetType().Name} can not be accessed as it is faulty.") { }
}
