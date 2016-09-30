using System.Linq;
using System.Threading.Tasks;
using Azuria.Community;
using Azuria.Utilities.Extensions;
using NUnit.Framework;

namespace Azuria.Test.Conference
{
    public class ConferenceInfoTest
    {
        private ConferenceInfo _conferenceInfo;

        #region Methods

        [Test]
        public void ConferenceTest()
        {
            Assert.IsNotNull(this._conferenceInfo.Conference);
            Assert.AreEqual(this._conferenceInfo.Conference.Id, 124536);
        }

        [Test]
        public void GetUnreadMessages()
        {
            Message[] lUnreadMessages = this._conferenceInfo.UnreadMessages.ToArray();
            Assert.IsNotEmpty(lUnreadMessages);
            Assert.AreEqual(lUnreadMessages.Length, 3);
        }

        [SetUp]
        public async Task Setup()
        {
            this._conferenceInfo =
                (await Community.Conference.GetConferences(GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess()).First
                    ();
            Assert.IsNotNull(this._conferenceInfo);
        }

        #endregion
    }
}