using Azuria.UserInfo.ControlPanel;
using NUnit.Framework;

namespace Azuria.Test.UcpTests
{
    [SetUpFixture]
    public class UcpSetup
    {
        public static UserControlPanel ControlPanel;

        #region Methods

        [OneTimeSetUp]
        public void Setup()
        {
            ControlPanel = new UserControlPanel(GeneralSetup.SenpaiInstance);
            Assert.IsNotNull(ControlPanel);
        }

        #endregion
    }
}