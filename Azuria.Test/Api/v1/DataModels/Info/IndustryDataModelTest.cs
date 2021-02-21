using System;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class IndustryDataModelTest : DataModelsTestBase<IndustryDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getindustry.json"];
            ProxerApiResponse<IndustryDataModel> lResponse = this.Convert(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result);
        }

        public static IndustryDataModel BuildDataModel()
        {
            return new IndustryDataModel
            {
                Country = Country.UnitedStates,
                Description = "Animax UK Description Test Text",
                Id = 1292,
                Link = new Uri("https://www.animaxtv.co.uk/"),
                Name = "Animax UK",
                Type = IndustryType.Streaming
            };
        }
    }
}