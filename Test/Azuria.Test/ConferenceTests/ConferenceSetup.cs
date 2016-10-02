using System;
using System.Threading.Tasks;
using Azuria.Community;
using Azuria.Utilities.Extensions;
using NUnit.Framework;

namespace Azuria.Test.ConferenceTests
{
    [SetUpFixture]
    public class ConferenceSetup
    {
        #region Methods

        [OneTimeSetUp]
        public async Task Setup()
        {
            await Conference.Init().ThrowFirstForNonSuccess();
            Conference.AutoCheckInterval = TimeSpan.FromMilliseconds(50);
        }

        #endregion
    }
}