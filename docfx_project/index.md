# Azuria - A Proxer.Me API in .NET (inofficial)

Master | NuGet | MyGet (Pre-Release Builds)
----- | ----- | -----
[![AppVeyor branch](https://img.shields.io/appveyor/ci/InfiniteSoul/Azuria/master.svg?maxAge=2592000?style=flat-square)](https://ci.appveyor.com/project/InfiniteSoul/azuria/branch/master) | [![NuGet](https://img.shields.io/nuget/v/Azuria.svg?style=flat-square)](https://www.nuget.org/packages/Azuria) | [![MyGet](https://img.shields.io/myget/infinitesoul/vpre/Azuria.svg)](https://www.myget.org/feed/infinitesoul/package/nuget/Azuria)

---

# Getting Started #

## Step 1. Installing ##
Installing Azuria is easy. Just download a compatible precompiled binary from the [GitHub release page](https://github.com/InfiniteSoul/Azuria/releases) and reference it in your project. Or you use NuGet to automaticaly find a compatible release for your project and install it. To install from NuGet just open the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console) and run the following command:

    PM> Install-Package Azuria

### Step 1.1 Using Pre-Release Packages (MyGet)
In order to use the pre-release packages available on MyGet you need to add one of the following nuget feed urls to your project:

**NuGet V3:**

	https://www.myget.org/F/infinitesoul/api/v3/index.json

**NuGet V2:**

	https://www.myget.org/F/infinitesoul/api/v2

All packages available on MyGet are automatically generated every time an automated build of the 'master' branch succeeded.


## Step 2. Initialisation (v0.6.1+)##
After installing you need to initialise the library to tell it some things that are needed to use the API like the API key. 

The API key is needed to access the API of Proxer.Me. It also dictates which requests you are allowed to make and which not. Further information and how to get a key can be found [here](https://proxer.me/wiki/Proxer_API/v1).

Passing the key to the library is really easy. Just call this somewhere at the start of your application:
```csharp
Azuria.Api.ApiInfo.Init(input =>
{
	input.ApiKeyV1 = "apiKey".ToCharArray();
});
```
Be sure to call this before any other methods of the library and pass the API key so that everything can be properly initialised.

## Special thanks
To the authors of the following libraries:

[JSON .NET](https://www.nuget.org/packages/Newtonsoft.Json/)
