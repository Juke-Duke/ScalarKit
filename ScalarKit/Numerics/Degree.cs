using System.Numerics;

namespace ScalarKit;

public readonly record struct Degree : IScalar<Degree, double>, IAdditionOperators<Degree, Degree, Degree>,
	ISubtractionOperators<Degree, Degree, Degree>
{
	private Degree(double degree)
		=> Value = degree;

	public static Degree operator +(Degree left, Degree right)
		=> left.Value + right.Value;

	public double Value { get; }

	public static implicit operator Degree(double degree)
		=> degree >= 0
			? new Degree(degree % 360)
			: throw new OverflowException($"{nameof(Degree)} value must be in range 0° to 360°.");

	public override string ToString()
		=> $"{Value}°";

	public static bool TryFrom(double primitive, out Degree scalar) => throw new NotImplementedException();

	public static Degree operator -(Degree left, Degree right)
		=> left.Value - right.Value;

	public static explicit operator Degree(string degree)
		=> double.TryParse(degree.TrimEnd('°', ' '), out double value)
			? value
			: throw new FormatException($"{nameof(Degree)} value must be in the format '000°'.");
}
