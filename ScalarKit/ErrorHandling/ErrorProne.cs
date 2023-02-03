using ScalarKit.Exceptions;

namespace ScalarKit.ErrorHandling;

public class ErrorProne<TValue, TError> : IErroneous<TError>, IScalar<ErrorProne<TValue, TError>, TValue>
    where TValue : notnull
    where TError : notnull
{
    protected readonly TValue _value = default!;

    protected readonly List<TError> _errors = new();

    public TValue Value => IsFaulty
        ? throw new FaultyValueException(_value)
        : _value;

    public IReadOnlyCollection<TError> Errors => _errors.AsReadOnly();

    public bool IsFaulty => _errors.Any() || _value is null;

    protected ErrorProne(TValue value)
        => _value = value;

    protected ErrorProne(TError error)
        => _errors = new() { error };

    public ErrorProne(IEnumerable<TError> errors)
        => _errors = new(errors);

    public static implicit operator ErrorProne<TValue, TError>(TValue value)
        => new(value);

    public static implicit operator ErrorProne<TValue, TError>(TError error)
        => new(error);

    public ErrorProne<TValue, TError> Inspect(Predicate<TValue> constraint, TError error)
    {
        ArgumentNullException.ThrowIfNull(_value);
        if (!constraint(_value))
            _errors.Add(error);

        return this;
    }

    public async Task<ErrorProne<TValue, TError>> InspectAsync(Func<TValue, Task<bool>> constraint, TError error)
    {
        ArgumentNullException.ThrowIfNull(_value);
        if (!await constraint(_value))
            _errors.Add(error);

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

    public async Task<TResult> DispatchAsync<TResult>(Func<TValue, Task<TResult>> onValue, Func<IReadOnlyCollection<TError>, Task<TResult>> onFaulty)
        => !IsFaulty
            ? await onValue(Value).ConfigureAwait(false)
            : await onFaulty(Errors).ConfigureAwait(false);

    public async Task DispatchSingleAsync(Func<TValue, Task> onValue, Func<TError, Task> onFaulty)
    {
        if (!IsFaulty)
            await onValue(Value).ConfigureAwait(false);
        else
            await onFaulty(Errors.First()).ConfigureAwait(false);
    }

    public async Task<TResult> DispatchSingleAsync<TResult>(Func<TValue, Task<TResult>> onValue, Func<TError, Task<TResult>> onFaulty)
        => !IsFaulty
            ? await onValue(Value).ConfigureAwait(false)
            : await onFaulty(Errors.First()).ConfigureAwait(false);

    public IEnumerator<TError> GetEnumerator()
        => _errors.GetEnumerator();

    public override string ToString()
        => !IsFaulty
            ? $"{_value}"
            : string.Join($",{Environment.NewLine}", _errors);
}

public sealed class ErrorProne<TValue> : ErrorProne<TValue, Exception>, IErroneous<Exception>, IScalar<ErrorProne<TValue>, TValue>
    where TValue : notnull
{
    private ErrorProne(TValue value)
        : base(value) { }

    private ErrorProne(Exception error)
        : base(error) { }

    public ErrorProne(IEnumerable<Exception> errors)
        : base(errors) { }

    public static implicit operator ErrorProne<TValue>(TValue value)
    {
        try { return new(value); }
            catch (Exception error) { return new(error); }
    }

    public static implicit operator ErrorProne<TValue>(Exception error)
        => new(error);
}
