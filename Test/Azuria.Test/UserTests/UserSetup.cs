using Azuria.UserInfo;
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
            User = new User(1);
        }

        #endregion
    }
}