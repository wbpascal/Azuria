# InitialisableProperty #

## Introduction ##
The `InitialisableProperty` class is like the `ProxerResult` class a core member of the library and you will need to use it regularly. It is used to get and cache different properties of different classes. **To understand some methods of this class it is recommended to first read the tutorial about the [ProxerResult](basic_classes_proxerresult.md) class.**

## Methods ##
As caching is done automatically you only need to know how to get the info out of the object. The following methods will help you in that task.

---

```csharp
ProxerResult<int> clicksResult = await anime.Clicks.GetObject();
//Alias (using the GetAwaiter() method):
ProxerResult<int> clicksResult = await anime.Clicks;
```
The `GetObject()` method returns a `ProxerResult` object with the type of the property. If the property does not contatin a value, the method will initialise it and if it does it will return the value wrapped in a `ProxerResult` object. This is the base method and is called by nearly every method as well. The method also contains an overload you can use as follows:
```csharp
int clicks = await anime.Clicks.GetObject(-1);
```
This overload will skip the wrapping in an result object step and will instead directly return the property type. If an exception is thrown during execution of the underlying method the exception is not thrown but instead the argument specified is returned.

---

```csharp
int clicks = await anime.Clicks.ThrowFirstOnNonSuccess();
```
This method does exactly like the name sounds. If the value could not be fetched from the public API the first exception that occured is thrown.

---

In case you want to force updating the value the `GetNewObject` method can be used.
```csharp
ProxerResult<int> clicksResult = await anime.Clicks.GetNewObject();
```
And like the `GetObject` method it also contains the overload:
```csharp
int clicks = await anime.Clicks.GetNewObject(-1);
```

## Properties ##
The `InitialisableProperty` class currently only contains a single property. This property is called `IsInitialisedOnce` returns a boolean that indicates whether the object already contains a cached value that is returned instead of fetching a new one. You don't normaly need to use this property as the methods handle this automatically.  