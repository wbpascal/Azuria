# Getting Started #

This will walk you through the steps of installing and setting up Azuria, so that you can go right ahead and use it. More advanced topics will be explained at a later point.

## Step 1. Installing ##
Installing Azuria is easy. Just download a compatible precompiled binary from the [GitHub release page](https://github.com/InfiniteSoul/Azuria/releases) and reference it in your project. Or you use NuGet to automaticaly find a compatible release for your project and install it. To install from NuGet just open the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console) and run the following command:

    PM> Install-Package Azuria -Pre

As of 9/21/2016 only the most recent pre-release package is compatible with this tutorial so be sure to append `-Pre` to the command as seen in the code above.

## Step 2. Initialisation ##
After installing you need to initialise the library to tell it some things that are needed to use the API. For now we will only look at the most importand argument, called the API key. Optional arguments will be explained in detail later on.

The API key is needed to access the API of Proxer.Me. It also dictates which requests you are allowed to make and which not. Further information and how to get a key can be found [here](https://proxer.me/wiki/Proxer_API/v1).

Passing the key to the library is really easy. Just call this at the start of your application:
```csharp
Azuria.Api.ApiInfo.Init(input =>
{
	input.ApiKeyV1 = "apiKey".ToCharArray();
});
```
Be sure to call this before any other methods of the library so that everything can be properly initialised. Why must the API key be converted to a char array, you ask? This too will be explained at a later point in time, specifically in the *Security* chapter.

## Thats it! ##
Now you can go ahead and use the entire library to your hearts content. But be sure to check out the other tutorials too because there a still some things you have to watch out for and some particularities of this library that you may or may not understand at the first glance. In particular the tutorials of the [ProxerResult](basic_classes_proxerresult.md) and the [InitialisableProperty](basic_classes_initialisableproperty.md) classes are worth a read.