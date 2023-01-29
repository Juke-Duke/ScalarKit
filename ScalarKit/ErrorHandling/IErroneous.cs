namespace ScalarKit.ErrorHandling;

public interface IErroneous<TError>
    where TError : notnull
{
    bool IsFaulty { get; }

    IReadOnlyCollection<TError> Errors { get; }
}
