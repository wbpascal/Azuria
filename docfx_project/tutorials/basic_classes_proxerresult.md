# ProxerResult #

## Introduction ##
The `ProxerResult` class aims to make handling exceptions easier as it will not always be clear which exceptions are thrown by a specific method and is a core class of the library. In this tutorial you will learn which properties and methods are available and how you can use them in your project.

## Getting results of methods ##
You surely already did come accross a method that returned a `ProxerResult` and wondered what to do with it. Lets take for example:

```csharp
ProxerResult<bool> proxerResult = await senpai.Login("password");
```

As you can see the `Login` method returns a `ProxerResult<bool>` and the result of the method is not immediately visible. The first thing you should do now is checking if the method even executed successfully by using

```csharp
if(proxerResult.Success){...}
```

If the method executed successfully you can now access the result of the method:

```csharp
bool result = proxerResult.Result;
```

Of course if the method did not execute successfully you can access the thrown exceptions by calling

```csharp
IEnumerable<Exception> exceptions = proxerResult.Exceptions;
```

All in all the full method call including error handling could now look something like this

```csharp
ProxerResult<bool> proxerResult = await senpai.Login("password");
if(proxerResult.Success)
{
	bool result = proxerResult.Result;
	...
}
else
{
	IEnumerable<Exception> exceptions = proxerResult.Exceptions;
	//Further error handling
}
```

## Using `Task<ProxerResult<T>>` ##
There are some extension methods available to ease the working flow with `Task<ProxerResult<T>>` or to incorporate them into existing exception handling.

```csharp
bool result = await senpai.Login("password").ThrowFirstForNonSuccess();
```

This will execute the given method and return the result of the `ProxerResult` directly. If the method did not execute the first thrown exception will be thrown by the extension method.