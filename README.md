![ScalarKit](ScalarKit.png)

# Getting Started

## `ErrorProne`
ScalarKit offers an alternative to error handling and the expensive throwing of exceptions, wrapping them in return objects called ErrorPrones.

### Creating an `ErrorProne`
There are two flavors of `ErrorProne`, one with a value and a list of Exceptions...
```cs
ErrorProne<int> proneInteger = 21;
proneInteger.Errors; // List of Exceptions
```
...and one with a value and a list of your own custom error types.
```cs
public record Error(string Code, string Message);
```
```cs
ErrorProne<int, Error> proneInteger = 420;
proneInteger.Errors; // List of Error objects
```
Both ways allow you to create an `ErrorProne` from:
- A value
- A starting error
- A list of errors
```cs
ErrorProne<int> proneInteger = 69;

ErrorProne<int> proneIntegerWithAnError = new Exception("I don't like this number...");

ErrorProne<int> proneIntegerWithErrors = new(new[]
{
    new Exception("I don't like this number..."),
    new ArgumentOutOfRangeException("Try a smaller number")
});
```
> **Note** The value or error type will be implicitly converted to the `ErrorProne` type, however, any `IEnumerable` of errors can only be passed in through the constructor. This is due to interfaces not being able to be implicitly convert to a type.

> **Warning** `ErrorProne` lives in a null free world, meaning neither the value nor the errors can be nullable.

### Using an ErrorProne
The method you will use often is `Inspect`, as this is how ErrorPrones build up their container of errors with a very fluent syntax.
```cs
public record User(Guid Id, string Username, string Password);
```
```cs
ErrorProne<User> proneUser = new User(Guid.NewGuid(), "John Doe");

proneUser
    .Inspect(
        constraint: user => user.Username.Contains(" "),
        error: new Exception("Username cannot contain spaces")
    )
    .Inspect(
        constraint: user => user.Password.Any(c => char.IsDigit(c)),
        error: new Exception("Password must contain at least one digit"
    );
```
After inspecting the value, you can check if the ErrorProne is valid or not.
```cs
if (proneUser.IsFaulty)
{
    // Do something with the value
}
else
{
    // Do something with the errors
}
```
There is also `Dispatch`, which allows for two functions to be utilized, one for the value and one for the errors.
```cs
// Do something with the first error...
proneUser.DispatchSingle(
    onValue: user => Console.WriteLine($"User {user.Username} is valid!"),
    onError: error => Console.WriteLine($"User is invalid: {error.Message}")
);

// ...or do something with all the errors
proneUser.Dispatch(
    onValue: user => Console.WriteLine($"User {user.Username} is valid!"),
    onError: errors => Console.WriteLine($"User is invalid, there are {user.Errors.Count} errors!")
);
```
Both `Inspect` and `Dispatch` offer asyncronous variants as well, `InspectAsync`, `DispatchSingleAsync` and `DispatchAsync`.

### Built in inspection methods
ScalarKit also offers a few built in fluent inspection methods for common validation checks on primitives:
- Any Type
    - `OneOf(IEnumerable<T> values, TError error)`
    - `NoneOf(IEnumerable<T> values, TError error)`
- Numbers
    - `GreaterThan(TNumber min, TError onOutOfBounds, bool includeMin = false)`
    - `LessThan(TNumber max, TError onOutOfBounds, bool includeMax = false)`
    - `InRange(TNumber min, TNumber max, TError onOutOfBounds, bool includeMin = false, bool includeMax = false)`
- Strings
    - `NotEmpty(TError onEmpty)`
    - `MinLength(int min, TError onOutOfBounds, bool includeMin = false)`
    - `MaxLength(int max, TError onOutOfBounds, bool includeMax = false)`
    - `BoundLength(int min, int max, TError onOutOfBounds, bool includeMin = false, bool includeMax = false)`

### `IErroneous<TError>`
`ErrorProne` implements `IErroneous<TError>`, providing a contract for types that can be faulty and contain a list of errors.
```cs
public interface IErroneous<TError>
    where TError : notnull
{
    bool IsFaulty { get; }

    IReadOnlyCollection<TError> Errors { get; }
}
```
This allows your own custom types to be used in the same way as `ErrorProne`. ScalarKit uses it to be able to implement the `AggregateErrors` and `AccumulateErrors` methods, allowing to group up `ErrorProne` objects with different values, but share the same error type.
```cs
public record AuthenticationResponse(
    string Username,
    string Email,
    string Password
);
```
```cs
ErrorProne<string> proneUsername = "John Doe";
ErrorProne<string> proneEmail = "johnDoe@gmail.com";
ErrorProne<string> pronePassword = "password123";

proneUsername.Inspect(...);
proneEmail.Inspect(...);
pronePassword.Inspect(...);

// approach 1
ErrorProne<AuthenticationResponse> proneAuthResponse = new(ErrorProne.AccumulateErrors(proneUsername, proneEmail, pronePassword));

// approach 2
proneAuthResponse = ErrorProne.AggregateErrors(
    proneUsername,
    proneEmail,
    pronePassword);
);

return proneAuthResponse.Dispatch(
    onValue: authResponse => authResponse.Value,
    onError: errors => authResponse
);
```
You can clean your instance of `ErrorProne` to only contain unique errors with a given comparator, for `ErrorProne<TValue>`, if a comparator for `Exception` is not provided, a built in comaparator will be used that compares the type and message of the exception.
```cs
ErrorProne<bool> alwaysTrue = true;
alwaysTrue
    .Inspect(
        constraint: _ => false,
        error: new Exception("NOT TRUE!")
    )
    .Inspect(
        constraint: _ => false,
        error: new Exception("NOT TRUE!")
    )
    .OnlyUniqueErrors(); // only contains one exception
```

### Accessing the value
The value of an `ErrorProne` can be accessed through the `Value` property, however, this will throw an exception if the `ErrorProne` is faulty, so use the `IsFaulty` property to check beforehand, or configure any of the `Dispatch` methods to handle it.
```cs
ErrorProne<int> proneInteger = 69;
proneInteger.LessThan(
    max: 50,
    onOutOfBounds: new ArgumentOutOfRangeException("Try a smaller number"),
    includeMax: true
);

Console.WriteLine(proneInteger.Value);
```
```bash
Unhandled exception. ScalarKit.Exceptions.FaultyValueException: The error prone Int32 can not be accessed as it is faulty.
```

> `ErrorProne` is inspired from functional railway oriented programming, as well as Rust's approach to error handling.
