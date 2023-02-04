using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace ScalarKit;

public readonly partial record struct Percentage : IScalar<Percentage, double>, IComparable, IComparable<Percentage>, IFormattable, IParsable<Percentage>, ISpanParsable<Percentage>, IAdditionOperators<Percentage, Percentage, Percentage>, ISubtractionOperators<Percentage, Percentage, Percentage>, IMultiplyOperators<Percentage, Percentage, Percentage>, IDivisionOperators<Percentage, Percentage, Percentage>, IModulusOperators<Percentage, Percentage, Percentage>, IIncrementOperators<Percentage>, IDecrementOperators<Percentage>, IUnaryPlusOperators<Percentage, Percentage>, IUnaryNegationOperators<Percentage, double>, IEqualityOperators<Percentage, Percentage, bool>, IComparisonOperators<Percentage, Percentage, bool>, IMinMaxValue<Percentage>
{
    public static Percentage MinValue => 0.0;

    public static Percentage MaxValue => 1.0;

    public double Value { get; }

    private Percentage(double percent)
        => Value = percent;

    public static implicit operator Percentage(double percent)
        => 0 <= percent && percent <= 1
            ? new Percentage(percent)
            : throw new OverflowException($"{nameof(Percentage)} value must be in range 0.0 to 1.0.");

    public static explicit operator Percentage(string percent)
        => double.TryParse(percent.TrimEnd(new char[] { '%', ' ' }), out var value)
            ? value / 100
            : throw new FormatException($"{nameof(Percentage)} value must be in the format '000%'.");

    public int CompareTo(Percentage value)
    {
        if (this > value)
            return 1;
        if (this < value)
            return -1;
        return 0;
    }

    public int CompareTo(object? value)
    {
        if (value is null)
            return 1;
        if (value is Percentage percent)
            return CompareTo(percent);

        throw new ArgumentException($"{nameof(value)} must be of type {nameof(Percentage)} to be compared.");
    }

    public static Percentage Parse(string percentString, IFormatProvider? provider)
        => (Percentage)percentString / 100;

    public static Percentage Parse(ReadOnlySpan<char> percentSpan, IFormatProvider? provider)
        => (Percentage)percentSpan.ToString() / 100;

    public static bool TryParse([NotNullWhen(true)] string? percentString, IFormatProvider? provider, [MaybeNullWhen(false)] out Percentage result)
    {
        if (percentString is null)
        {
            result = 0.0;
            return false;
        }

        try
        {
            result = Parse(percentString, provider);
            return true;
        }
        catch
        {
            result = 0.0;
            return false;
        }
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Percentage result)
        => TryParse(s.ToString(), provider, out result);

    public override string ToString()
        => $"{Value * 100} %";

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (format is null)
            return ToString();
        if (format.StartsWith("P"))
            return $"{Value.ToString(format, formatProvider)}";

        throw new FormatException($"Only 'P' format is supported.");
    }

    public static Percentage operator +(Percentage left, Percentage right)
        => left.Value + right.Value;

    public static Percentage operator -(Percentage left, Percentage right)
        => left.Value - right.Value > 0
            ? left.Value - right.Value
            : throw new OverflowException("Arithmetic operation resulted in an overflow.");

    public static Percentage operator *(Percentage left, Percentage right)
        => left.Value * right.Value;

    public static Percentage operator /(Percentage left, Percentage right)
        => left.Value / right.Value;

    public static Percentage operator %(Percentage left, Percentage right)
        => left.Value % right.Value;

    public static Percentage operator ++(Percentage value)
        => value.Value + 0.1;

    public static Percentage operator --(Percentage value)
        => value.Value - 0.1 > 0
            ? value.Value - 0.1
            : throw new OverflowException("Arithmetic operation resulted in an overflow.");

    public static Percentage operator +(Percentage value)
        => value;

    public static double operator -(Percentage value)
        => -value.Value;

    public static bool operator <(Percentage left, Percentage right)
        => left.Value < right.Value;

    public static bool operator >(Percentage left, Percentage right)
        => left.Value > right.Value;

    public static bool operator <=(Percentage left, Percentage right)
        => left.Value <= right.Value;

    public static bool operator >=(Percentage left, Percentage right)
        => left.Value >= right.Value;
}
