namespace ScalarKit.ErrorHandling;

public interface IErroneous<TError>
    where TError : notnull
{
    bool IsFaulty { get; }

    IReadOnlySet<TError> Errors { get; }
}
