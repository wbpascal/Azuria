using System.Linq;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.ErrorHandling;
using Azuria.Helpers;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Messenger
{
    public class ConferenceDataModelTest : DataModelsTestBase<ConferenceDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["messenger_getconferences.json"];
            ProxerApiResponse<ConferenceDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(3, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        private static ConferenceDataModel BuildDataModel()
        {
            return new ConferenceDataModel
            {
                ConferenceId = 124536,
                ConferenceImage = null,
                ConferenceTopic = "Proxer API - Diskussion",
                ConferenceTopicCustom = "",
                IsConferenceGroup = true,
                IsLastMessageRead = false,
                LastMessageTimeStamp = DateTimeHelpers.UnixTimeStampToDateTime(1474469535),
                LastReadMessageId = 5018808,
                ParticipantsCount = 10,
                UnreadMessagesCount = 3
            };
        }
    }
}