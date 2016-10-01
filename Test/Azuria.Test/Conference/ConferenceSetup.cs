using System;
using System.Threading.Tasks;
using Azuria.Utilities.Extensions;
using NUnit.Framework;

namespace Azuria.Test.Conference
{
    [SetUpFixture]
    public class ConferenceSetup
    {
        #region Methods

        [OneTimeSetUp]
        public async Task Setup()
        {
            await Community.Conference.Init().ThrowFirstForNonSuccess();
            Community.Conference.AutoCheckInterval = TimeSpan.FromMilliseconds(50);
        }

        #endregion
    }
}