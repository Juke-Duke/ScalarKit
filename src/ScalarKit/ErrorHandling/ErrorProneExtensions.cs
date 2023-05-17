using System.Numerics;
using System.Text.RegularExpressions;

namespace ScalarKit.ErrorHandling;

public static class ErrorProne
{
	public static void ThrowFirstError(this IErroneous<Exception> prones)
	{
		if (prones.IsFaulty)
			throw prones.Errors.First();
	}

	public static bool AnyFaulty<TError>(params IErroneous<TError>[] prones)
		where TError : notnull
		=> prones.Any(e => e.IsFaulty);

	public static ErrorProne<TValue, TError> DistinctErrors<TValue, TError>(
		this ErrorProne<TValue, TError> proneValue,
		IEqualityComparer<TError>? errorComparer = null
	)
		where TValue : notnull
		where TError : notnull
		=> new(proneValue.Errors.Distinct(errorComparer));

	public static ErrorProne<TValue> DistinctErrors<TValue>(
		this ErrorProne<TValue> proneValue,
		IEqualityComparer<Exception>? exceptionComparer = null
	)
		where TValue : notnull
		=> new(proneValue.Errors.Distinct(exceptionComparer ?? new ExceptionEqualityComparer()));

	public static ErrorProne<TValue, TError> OneOf<TValue, TError>(
		this ErrorProne<TValue, TError> proneValue,
		IReadOnlySet<TValue> values,
		TError onNoneOf
	)
		where TValue : notnull, IEquatable<TValue>
		where TError : notnull
		=> proneValue.Inspect(
			v => values.Contains(v),
			onNoneOf
		);

	public static ErrorProne<TValue, TError> NoneOf<TValue, TError>(
		this ErrorProne<TValue, TError> proneValue,
		IReadOnlySet<TValue> values,
		TError onOneOf
	)
		where TValue : notnull, IEquatable<TValue>
		where TError : notnull
		=> proneValue.Inspect(
			v => !values.Contains(v),
 			onOneOf
		);

	public static ErrorProne<TNumber, TError> GreaterThan<TNumber, TError>(
		this ErrorProne<TNumber, TError> proneNumber,
		TNumber min,
		TError onOutOfBounds,
		bool includeMin = false
	)
		where TNumber : INumber<TNumber>
		where TError : notnull
		=> proneNumber.Inspect(
			number => includeMin ? number >= min : number > min,
			onOutOfBounds
		);

	public static ErrorProne<TNumber, TError> LessThan<TNumber, TError>(
		this ErrorProne<TNumber, TError> proneNumber,
		TNumber max,
		TError onOutOfBounds,
		bool includeMax = false
	)
		where TNumber : INumber<TNumber>
		where TError : notnull
		=> proneNumber.Inspect(
			number => includeMax ? number <= max : number < max,
			onOutOfBounds
		);

	public static ErrorProne<TNumber, TError> Between<TNumber, TError>(
		this ErrorProne<TNumber, TError> proneNumber,
		TNumber min,
		TNumber max,
		TError onOutOfBounds,
		bool includeMin = false,
		bool includeMax = false
	)
		where TNumber : INumber<TNumber>
		where TError : notnull
		=> max < min
			? throw new ArgumentException($"{nameof(max)} must be greater than {nameof(min)}")
			: proneNumber
			   .GreaterThan(min, onOutOfBounds, includeMin)
			   .LessThan(max, onOutOfBounds, includeMax);

	public static ErrorProne<string, TError> NotEmpty<TError>(
		this ErrorProne<string, TError> proneValue, TError onEmpty
	)
		where TError : notnull
		=> proneValue.Inspect(
			s => !string.IsNullOrEmpty(s),
			onEmpty
		);

	public static ErrorProne<string, TError> MinLength<TError>(
		this ErrorProne<string, TError> proneValue,
		int minLength,
		TError onOutOfBounds,
		bool includeMin = false
	)
		where TError : notnull
		=> proneValue.Inspect(
			s => includeMin ? s.Length >= minLength : s.Length > minLength,
			onOutOfBounds
		);

	public static ErrorProne<string, TError> MaxLength<TError>(
		this ErrorProne<string, TError> proneValue,
		int maxLength,
		TError onOutOfBounds,
		bool includeMax = false
	)
		where TError : notnull
		=> proneValue.Inspect(
			s => includeMax ? s.Length <= maxLength : s.Length < maxLength,
			onOutOfBounds
		);

	public static ErrorProne<string, TError> BoundLength<TError>(
		this ErrorProne<string, TError> proneValue,
		int minLength,
		int maxLength,
		TError onOutOfBounds,
		bool includeMin = false,
		bool includeMax = false
	)
		where TError : notnull
		=> maxLength < minLength
			? throw new ArgumentException($"{nameof(maxLength)} must be greater than {nameof(minLength)}")
			: proneValue
			   .MinLength(minLength, onOutOfBounds, includeMin)
			   .MaxLength(maxLength, onOutOfBounds, includeMax);

	public static ErrorProne<string, TError> Matches<TError>(
		this ErrorProne<string, TError> proneValue,
		Regex pattern,
		TError onMismatch
	)
		where TError : notnull
		=> proneValue.Inspect(
			str => pattern.IsMatch(str),
			onMismatch
		);

	public static ErrorProne<DateTime, TError> Before<TError>(
		this ErrorProne<DateTime, TError> proneValue,
		DateTime max,
		TError onOutOfBounds,
		bool includeMax = false
	)
		where TError : notnull
		=> proneValue.Inspect(
			date => includeMax ? date <= max : date < max,
			onOutOfBounds
		);

	public static ErrorProne<DateTime, TError> After<TError>(
		this ErrorProne<DateTime, TError> proneValue,
		DateTime min,
		TError onOutOfBounds,
		bool includeMin = false
	)
		where TError : notnull
		=> proneValue.Inspect(
			date => includeMin ? date >= min : date > min,
			onOutOfBounds
		);

	public static ErrorProne<DateTime, TError> Between<TError>(
		this ErrorProne<DateTime, TError> proneValue,
		DateTime min,
		DateTime max,
		TError onOutOfBounds,
		bool includeMin = false,
		bool includeMax = false
	)
		where TError : notnull
		=> max < min
			? throw new ArgumentException($"{nameof(max)} must be greater than {nameof(min)}")
			: proneValue
			   .After(min, onOutOfBounds, includeMin)
			   .Before(max, onOutOfBounds, includeMax);
}
