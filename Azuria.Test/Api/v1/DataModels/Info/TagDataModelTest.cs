using System;
using Azuria.Api.v1.DataModels.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using Xunit;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    public class TagDataModelTest : DataModelsTestBase<TagDataModel>
    {
        [Fact]
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
                Assert.Equal(id, dataModel.Id);
                Assert.Equal(tagId, dataModel.TagId);
                Assert.Equal(timestamp, dataModel.Timestamp);
                Assert.Equal(isRated, dataModel.IsRated);
                Assert.Equal(isSpoiler, dataModel.IsSpoiler);
                Assert.Equal(name, dataModel.Name);
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