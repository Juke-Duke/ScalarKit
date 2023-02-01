using System.Numerics;

namespace ScalarKit;

public readonly record struct Degree : IScalar<Degree, double>, IAdditionOperators<Degree, Degree, Degree>, ISubtractionOperators<Degree, Degree, Degree>
{
    public double Value { get; }

    private Degree(double degree)
        => Value = degree;

    public static implicit operator Degree(double degree)
        => 0 <= degree
            ? new Degree(degree % 360)
            : throw new OverflowException($"{nameof(Degree)} value must be positive.");

    public static explicit operator Degree(string degree)
        => double.TryParse(degree.TrimEnd(new char[] { '°', ' ' }), out var value)
            ? value
            : throw new FormatException($"{nameof(Degree)} value must be in the format '000°'.");

    public override string ToString()
        => $"{Value}°";

    public static Degree operator +(Degree left, Degree right)
        => left.Value + right.Value;

    public static Degree operator -(Degree left, Degree right)
        => left.Value - right.Value;
}
