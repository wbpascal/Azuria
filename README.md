# Azuria - A Proxer.Me API in .NET (inofficial)

##Status
Master-Branch: [![Build status](https://ci.appveyor.com/api/projects/status/eenr5ksrjakegl0e/branch/master?svg=true)](https://ci.appveyor.com/project/InfiniteSoul/massive-octo-wookie/branch/master)

Newest Commit: [![Build status](https://ci.appveyor.com/api/projects/status/eenr5ksrjakegl0e?svg=true)](https://ci.appveyor.com/project/InfiniteSoul/massive-octo-wookie)

Issues: [![Stories in Ready](https://badge.waffle.io/InfiniteSoul/azuria.svg?label=ready&title=Ready)](http://waffle.io/InfiniteSoul/azuria)

Documentation: http://azuria.infinitesoul.de/help (only available in German atm)

---

##Warning!
Because this Class Library implements not only the official API of Proxer.Me it is dependent on the Layout of the Website too. This implies that every change that is being made to the website, even little ones, can break parts or the entirety of this Class Library. If under any circumstances problems arise in result of the aforementioned problem, I ask you to report it to the  [issue Page](https://github.com/InfiniteSoul/Azuria/issues).

---

##What does this do?
Azuria is a **inofficial** Class Library, which exposes the functions of the official Proxer.Me API as well as a lot more to .NET compatible languages. The whole project consists at the moment of a "normal" class library in .NET 4.5, which means, albeit not tested enough, that it can be used in a Mono environment, and a portable version. 


##Ok nice! Then... How do I install it?
The currently most reliable method is to install it via NuGet. To install it via NuGet you have to open a compatible console (for example the build-in one in visual studio) and type in:
```
PM> Install-Package Azuria
```

##The `ProxerResult` class
This class is used as a return type in a lot of methods and aims to help with handling errors encountered during runtime. The class exposes the following important members:

####The `Success` Property 
As the name may entail it returns a true boolean value if the method was a success and a false one if it wasn't. If the method failed to execute more information as to why it did can be found in the `Exceptions` property.

####The `Exceptions` Property
Like mentioned in the description of the `Success` property this property returns all exceptions that happened during the execution of the method. This does not always mean the method failed! The only realiable method to check whether the method failed is to look at the `Success` property.

####The `Result` Property (Only available in the `ProxerResult<T>` subclass)
This property returns the result of the method if it returns anything else than `ProxerResult` or `Task<ProxerResult>`. The returned result is always of type T if `Success` is true and null if not.

####The `OnError(T)` Method (Only available in the `ProxerResult<T>` subclass)
If the method failed to execute and as a result the `Success` property returns false, this method automatically returns a specified object of type T. If the method executed normaly without problems it just returns the value specified in `Result`. Example in C#:
```csharp
bool loggedIn = (await senpai.Login("benutzername", "passwort")).OnError(false);
```


##Still questions? 
Then take a look at the [documentation] (http://azuria.infinitesoul.de/help) (documentation only available in german at the moment).

## Special thanks
To the authors of the following libraries:

[JSON .NET](https://www.nuget.org/packages/Newtonsoft.Json/)

[HtmlAgilityPack](https://htmlagilitypack.codeplex.com/)

[RestSharp](http://restsharp.org/)
