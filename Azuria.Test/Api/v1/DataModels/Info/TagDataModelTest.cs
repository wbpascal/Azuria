using System;
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
            this.CheckSuccessResponse(lResponse);
            CheckDataModels(lResponse.Result);
        }

        public static void CheckDataModels(TagDataModel[] dataModels)
        {
            void CheckDataModel(
                TagDataModel dataModel, int id, int tagId, DateTime timestamp, bool isRated, bool isSpoiler,
                string name, string description)
            {
                Assert.AreEqual(id, dataModel.Id);
                Assert.AreEqual(tagId, dataModel.TagId);
                Assert.AreEqual(timestamp, dataModel.Timestamp);
                Assert.AreEqual(isRated, dataModel.IsRated);
                Assert.AreEqual(isSpoiler, dataModel.IsSpoiler);
                Assert.AreEqual(name, dataModel.Name);
            }

            CheckDataModel(
                dataModels[0], 174, 257, new DateTime(2016, 6, 17, 15, 11, 16), false, true, "Bad End",
                "Bad End Beschreibungstext");
            CheckDataModel(
                dataModels[1], 3949, 99, new DateTime(2016, 6, 19, 16, 31, 49), true, false, "Osananajimi",
                "Osananajimi Beschreibung");
        }
    }
}