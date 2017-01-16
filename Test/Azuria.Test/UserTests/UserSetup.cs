using System.Threading.Tasks;
using Azuria.UserInfo;
using Azuria.Utilities.Extensions;
using NUnit.Framework;

namespace Azuria.Test.UserTests
{
    [SetUpFixture]
    public class UserSetup
    {
        #region Properties

        public static User User { get; private set; }

        #endregion

        #region Methods

        [OneTimeSetUp]
        public void Setup()
        {
            Task<User> lUserCreateTask = User.FromId(1).ThrowFirstForNonSuccess();
            lUserCreateTask.Wait();
            User = lUserCreateTask.Result;
        }

        #endregion
    }
}