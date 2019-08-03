using System;
using System.Linq;
using Azuria.Api.v1.DataModels.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class TagDataModelTest : DataModelsTestBase<TagDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getentrytags.json"];
            ProxerApiResponse<TagDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        public static TagDataModel BuildDataModel()
        {
            return new TagDataModel
            {
                Description = "Bad End Beschreibungstext",
                Id = 174,
                IsRated = false,
                IsSpoiler = true,
                Name = "Bad End",
                TagId = 257,
                Timestamp = new DateTime(2016, 6, 17, 15, 11, 16)
            };
        }
    }
}