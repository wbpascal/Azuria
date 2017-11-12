using Azuria.Api.v1.DataModels.Messenger;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Messenger
{
    public class ConstantsDataModelTest : DataModelsTestBase<ConstantsDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["messenger_getconstants.json"];
            ProxerApiResponse<ConstantsDataModel> lResponse = this.Convert(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result);
        }

        private static ConstantsDataModel BuildDataModel()
        {
            return new ConstantsDataModel
            {
                ConferencesPerPage = 48,
                MaxCharactersPerMessage = 65_000,
                MaxCharactersTopic = 32,
                MaxUsersPerConference = 100,
                MessagesPerPage = 30
            };
        }
    }
}