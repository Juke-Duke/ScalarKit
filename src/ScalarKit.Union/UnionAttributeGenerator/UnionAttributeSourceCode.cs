namespace ScalarKit.Union.UnionAttributeGenerator;

public static class UnionAttributeSourceCode
{
	public const string UnionAttribute = """
    using System;

    namespace ScalarKit.Union;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class UnionAttribute : Attribute
    {
        public UnionAttribute(Type type, params Type[] rest) { }
    }
    """;
}
