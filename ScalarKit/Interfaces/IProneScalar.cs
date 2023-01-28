using ScalarKit.ErrorHandling;

namespace ScalarKit;

public interface IProneScalar<TSelf, TPrimitive, TError> : IScalar<TSelf, TPrimitive>
    where TSelf : notnull, IScalar<TSelf, TPrimitive>
    where TPrimitive : notnull
    where TError : notnull
{
    static abstract ErrorProne<TSelf, TError> Inspect(TPrimitive primitive);
}

public interface IProneScalar<TSelf, TPrimitive> : IScalar<TSelf, TPrimitive>
    where TSelf : notnull, IScalar<TSelf, TPrimitive>
    where TPrimitive : notnull
{
    static abstract ErrorProne<TSelf> Inspect(TPrimitive primitive);
}
