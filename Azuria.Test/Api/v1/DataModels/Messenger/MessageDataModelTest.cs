using System.Linq;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Enums.Messenger;
using Azuria.ErrorHandling;
using Azuria.Helpers;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Messenger
{
    public class MessageDataModelTest : DataModelsTestBase<MessageDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["messenger_getmessages.json"];
            ProxerApiResponse<MessageDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(2, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        private static MessageDataModel BuildDataModel()
        {
            return new MessageDataModel
            {
                ConferenceId = 124536,
                MessageAction = MessageAction.NoAction,
                MessageContent = "Lorem ipsum dolor sit amet",
                MessageId = 4993930,
                MessageTimeStamp = DateTimeHelpers.UnixTimeStampToDateTime(1473602860),
                SenderDevice = "default",
                SenderUserId = 121658,
                SenderUsername = "RubyDerBoss"
            };
        }
    }
}