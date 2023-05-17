using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ScalarKit.Extensions.Serialization.HotChocolateSerialization;

public static class RequestExecutorBuilderExtensions
{
	public static IRequestExecutorBuilder BindScalarKitTypes(this IRequestExecutorBuilder builder)
	{
		builder.BindRuntimeType<EmailAddress, EmailAddressType>();
        builder.AddTypeConverter<EmailAddress, string>(e => e.Value);
        builder.AddTypeConverter<string, EmailAddress>(s => (EmailAddress)s);

		builder.BindRuntimeType<Username, UsernameType>();
        builder.AddTypeConverter<Username, string>(u => u.Value);
        builder.AddTypeConverter<string, Username>(s => (Username)s);

		return builder;
	}
}
