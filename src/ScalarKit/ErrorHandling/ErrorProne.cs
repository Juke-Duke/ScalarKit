using ScalarKit.Exceptions;

namespace ScalarKit.ErrorHandling;

public class ErrorProne<TValue, TError> : IErroneous<TError>
	where TValue : notnull
	where TError : notnull
{
	protected readonly TValue value = default!;

	protected readonly List<TError> errors = new();

	public TValue Value => IsFaulty
		? throw new FaultyValueException(value)
		: value;

	public IReadOnlyCollection<TError> Errors => errors.AsReadOnly();

	public bool IsFaulty => errors.Any() || value is null;

	protected ErrorProne(TValue value)
		=> this.value = value;

	protected ErrorProne(TError error)
		=> errors = new List<TError> { error };

	public ErrorProne(IEnumerable<TError> errors)
		=> this.errors = new List<TError>(errors);

	public ErrorProne(IErroneous<TError> prone, params IErroneous<TError>[] prones)
		=> errors = prone.Errors.Concat(prones.SelectMany(p => p.Errors)).ToList();

	public static implicit operator ErrorProne<TValue, TError>(TValue value)
		=> new(value);

	public static implicit operator ErrorProne<TValue, TError>(TError error)
		=> new(error);

	public ErrorProne<TValue, TError> Inspect(Predicate<TValue> constraint, TError error)
	{
		if (value is null || !constraint(value))
			errors.Add(error);

		return this;
	}

	public async Task<ErrorProne<TValue, TError>> InspectAsync(Func<TValue, Task<bool>> constraint, TError error)
	{
		if (value is null || !await constraint(value))
			errors.Add(error);

		return this;
	}

	public void Dispatch(Action<TValue> onValue, Action<IReadOnlyCollection<TError>> onFaulty)
	{
		if (!IsFaulty)
			onValue(Value);
		else
			onFaulty(Errors);
	}

	public TResult Dispatch<TResult>(Func<TValue, TResult> onValue, Func<IReadOnlyCollection<TError>, TResult> onFaulty)
		=> !IsFaulty
			? onValue(Value)
			: onFaulty(Errors);

	public void DispatchSingle(Action<TValue> onValue, Action<TError> onFaulty)
	{
		if (!IsFaulty)
			onValue(Value);
		else
			onFaulty(Errors.First());
	}

	public TResult DispatchSingle<TResult>(Func<TValue, TResult> onValue, Func<TError, TResult> onFaulty)
		=> !IsFaulty
			? onValue(Value)
			: onFaulty(Errors.First());

	public async Task DispatchAsync(Func<TValue, Task> onValue, Func<IReadOnlyCollection<TError>, Task> onFaulty)
	{
		if (!IsFaulty)
			await onValue(Value).ConfigureAwait(false);
		else
			await onFaulty(Errors).ConfigureAwait(false);
	}

	public async Task<TResult> DispatchAsync<TResult>(
		Func<TValue, Task<TResult>> onValue, Func<IReadOnlyCollection<TError>, Task<TResult>> onFaulty
	) => !IsFaulty
		? await onValue(Value).ConfigureAwait(false)
		: await onFaulty(Errors).ConfigureAwait(false);

	public async Task DispatchSingleAsync(Func<TValue, Task> onValue, Func<TError, Task> onFaulty)
	{
		if (!IsFaulty)
			await onValue(Value).ConfigureAwait(false);
		else
			await onFaulty(Errors.First()).ConfigureAwait(false);
	}

	public async Task<TResult> DispatchSingleAsync<TResult>(
		Func<TValue, Task<TResult>> onValue, Func<TError, Task<TResult>> onFaulty
	) => !IsFaulty
		? await onValue(Value).ConfigureAwait(false)
		: await onFaulty(Errors.First()).ConfigureAwait(false);

	public override string ToString()
		=> !IsFaulty
			? $"{value}"
			: string.Join($",\n", errors);
}

public sealed class ErrorProne<TValue> : ErrorProne<TValue, Exception>, IErroneous<Exception>
	where TValue : notnull
{
	private ErrorProne(TValue value)
		: base(value) { }

	private ErrorProne(Exception error)
		: base(error) { }

	public ErrorProne(IEnumerable<Exception> errors)
		: base(errors) { }

	public ErrorProne(IErroneous<Exception> prone, params IErroneous<Exception>[] prones)
		: base(prone, prones) { }

	public static implicit operator ErrorProne<TValue>(TValue value)
	{
		try { return new ErrorProne<TValue>(value); }
		catch (Exception error) { return new ErrorProne<TValue>(error); }
	}

	public static implicit operator ErrorProne<TValue>(Exception error)
		=> new(error);
}
