using Azuria.UserInfo.ControlPanel;
using NUnit.Framework;

namespace Azuria.Test.UcpTests
{
    [SetUpFixture]
    public class UcpSetup
    {
        #region Properties

        public static UserControlPanel ControlPanel { get; private set; }

        #endregion

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