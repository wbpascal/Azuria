using Azuria.Api.v1.DataModels.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using Xunit;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    public class FullEntryDataModelTest : DataModelsTestBase<FullEntryDataModel>
    {
        [Fact]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getfullentry.json"];
            ProxerApiResponse<FullEntryDataModel> lResponse = this.Convert(lJson);
            this.CheckSuccessResponse(lResponse);
            EntryDataModelTest.CheckDataModel(lResponse.Result);
            IndustryBasicDataModelTest.CheckDataModels(lResponse.Result.Industry);
            NameDataModelTest.CheckDataModels(lResponse.Result.Names);
            SeasonDataModelTest.CheckDataModels(lResponse.Result.Seasons, true);
            TagDataModelTest.CheckDataModels(lResponse.Result.Tags);
            TranslatorBasicDataModelTest.CheckDataModels(lResponse.Result.Translator);
        }
    }
}