using Azuria.Api.v1.DataModels.Messenger;
using Azuria.ErrorHandling;
using Azuria.Helpers;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Messenger
{
    public class ConferenceInfoDataModelTest : DataModelsTestBase<ConferenceInfoDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["messenger_getconferenceinfo.json"];
            ProxerApiResponse<ConferenceInfoDataModel> lResponse = this.Convert(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result);
        }

        private static ConferenceInfoDataModel BuildDataModel()
        {
            return new ConferenceInfoDataModel
            {
                MainInfo = new ConferenceInfoMainDataModel
                {
                    FirstMessageTimeStamp = DateTimeHelpers.UnixTimeStampToDateTime(1472560334),
                    LastMessageTimeStamp = DateTimeHelpers.UnixTimeStampToDateTime(1475001736),
                    LeaderUserId = 121658,
                    Title = "Proxer API - Diskussion",
                    UserCount = 2
                },
                ParticipantsInfo = new[]
                {
                    new ConferenceInfoParticipantDataModel
                    {
                        AvatarId = "391004_Kr4LXO.jpg",
                        UserId = 391004,
                        Username = "cuechan",
                        UserStatus = "1462630115"
                    },
                    new ConferenceInfoParticipantDataModel
                    {
                        AvatarId = "121658_VHuZqz.jpg",
                        UserId = 121658,
                        Username = "RubyDerBoss",
                        UserStatus = "Proxer-App Testversion: https://github.com/proxer/ProxerAndroid"
                    }
                }
            };
        }
    }
}