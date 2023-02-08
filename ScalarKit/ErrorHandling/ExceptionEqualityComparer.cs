using System.Diagnostics.CodeAnalysis;

namespace ScalarKit.ErrorHandling;

public readonly struct ExceptionEqualityComparer : IEqualityComparer<Exception>
{
	public bool Equals(Exception? exception1, Exception? exception2)
	{
		if (exception1 is null && exception2 is null)
			return true;

		if (exception1 is null || exception2 is null)
			return false;

		return exception1.GetType() == exception2.GetType()
		 && exception1.Message == exception2.Message;
	}

	public int GetHashCode([DisallowNull] Exception exception)
		=> HashCode.Combine(exception.GetType(), exception.Message);
}
