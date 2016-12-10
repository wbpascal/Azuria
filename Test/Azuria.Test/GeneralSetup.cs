using System.Threading.Tasks;
using Azuria;
using Azuria.Api;
using Azuria.Security;
using Azuria.Test.Core;
using Azuria.Utilities.Extensions;
using NUnit.Framework;

[SetUpFixture]
// ReSharper disable once CheckNamespace
public class GeneralSetup
{
    #region Properties

    public static Senpai SenpaiInstance { get; set; }

    #endregion

    #region Methods

    public void InitApi()
    {
        ApiInfo.Init(input =>
        {
            input.ApiKeyV1 = "apiKey".ToCharArray();
            input.CustomHttpClient = senpai => new TestingHttpClient(senpai);
        });
    }

    public async Task InitSenpaiInstance()
    {
        SenpaiInstance = await Senpai.FromCredentials(
                new ProxerCredentials("InfiniteSoul", "correct".ToCharArray()))
            .ThrowFirstForNonSuccess();
        Assert.IsTrue(SenpaiInstance.IsProbablyLoggedIn);
    }

    [OneTimeSetUp]
    public void Setup()
    {
        this.InitApi();
        this.InitSenpaiInstance().Wait();
    }

    #endregion
}