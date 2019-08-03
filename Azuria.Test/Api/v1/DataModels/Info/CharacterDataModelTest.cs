using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class CharacterDataModelTest : DataModelsTestBase<CharacterDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getcharacters.json"];
            ProxerApiResponse<CharacterDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(BuildDataModels(), lResponse.Result);
        }

        public static CharacterDataModel[] BuildDataModels()
        {
            return new[]
            {
                new CharacterDataModel
                {
                    Id = 518,
                    Name = "Kaori Miyazono",
                    Role = CharacterRole.Main
                },
                new CharacterDataModel
                {
                    Id = 2446,
                    Name = "Emi Igawa",
                    Role = CharacterRole.Support
                }
            };
        }
    }
}