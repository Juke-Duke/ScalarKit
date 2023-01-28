namespace ScalarKit;

public interface IScalar<TSelf, TPrimitive>
    where TSelf : notnull, IScalar<TSelf, TPrimitive>
    where TPrimitive : notnull
{
    TPrimitive Value { get; }

    static abstract implicit operator TSelf(TPrimitive primitive);

    string? ToString()
        => Value.ToString();
}
