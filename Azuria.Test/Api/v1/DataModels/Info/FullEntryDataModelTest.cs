using Azuria.Api.v1.DataModels.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class FullEntryDataModelTest : DataModelsTestBase<FullEntryDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getfullentry.json"];
            ProxerApiResponse<FullEntryDataModel> lResponse = this.Convert(lJson);
            this.CheckSuccessResponse(lResponse);

            FullEntryDataModel lDataModel = lResponse.Result;
            EntryDataModelTest.CheckDataModel(lDataModel);
            IndustryBasicDataModelTest.CheckDataModels(lDataModel.Industry);
            NameDataModelTest.CheckDataModels(lDataModel.Names);
            SeasonDataModelTest.CheckDataModels(lDataModel.Seasons, true);
            TagDataModelTest.CheckDataModels(lDataModel.Tags);
            TranslatorBasicDataModelTest.CheckDataModels(lDataModel.Translator);

            Assert.False(lDataModel.IsHContent);
        }
    }
}