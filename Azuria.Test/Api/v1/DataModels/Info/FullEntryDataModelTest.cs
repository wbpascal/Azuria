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
            PublisherDataModelTest.CheckDataModels(lResponse.Result.Publisher);
            NameDataModelTest.CheckDataModels(lResponse.Result.Names);
        }
    }
}