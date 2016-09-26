using System.Threading.Tasks;
using Azuria.Api;
using Azuria.Utilities.Extensions;
using NUnit.Framework;

namespace Azuria.Test
{
    [SetUpFixture]
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
            SenpaiInstance = new Senpai("username");
            await SenpaiInstance.Login("correct").ThrowFirstForNonSuccess();
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
}