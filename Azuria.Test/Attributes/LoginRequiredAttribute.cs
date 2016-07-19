using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Azuria.Test.Attributes
{
    public class LoginRequiredAttribute : NUnitAttribute, ITestAction
    {
        #region Inherited

        /// <summary>Executed before each test is run</summary>
        /// <param name="test">The test that is going to be run.</param>
        public void BeforeTest(ITest test)
        {
            Api.ApiInfo.InitV1(Credentials.ApiKey);
            if (!SenpaiTest.Senpai.IsProbablyLoggedIn)
                SenpaiTest.Senpai.Login(Credentials.Password).Wait();
        }

        /// <summary>Executed after each test is run</summary>
        /// <param name="test">The test that has just been run.</param>
        public void AfterTest(ITest test)
        {
        }

        /// <summary>Provides the target for the action attribute</summary>
        /// <returns>The target for the action attribute</returns>
        public ActionTargets Targets => ActionTargets.Default;

        #endregion
    }
}