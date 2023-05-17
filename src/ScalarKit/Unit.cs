namespace ScalarKit;

#pragma warning disable CS8981
public readonly struct unit
{
	public static unit Unit(Action action)
	{
		action();
		return default;
	}
}
#pragma warning restore CS8981
