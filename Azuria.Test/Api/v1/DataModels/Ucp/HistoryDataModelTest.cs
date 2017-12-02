using System;
using System.Linq;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Ucp
{
    public class HistoryDataModelTest : DataModelsTestBase<HistoryDataModelBase>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["ucp_gethistory.json"];
            ProxerApiResponse<HistoryDataModelBase[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(3, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        private static HistoryDataModelBase BuildDataModel()
        {
            return new HistoryDataModelBase
            {
                ContentIndex = 23,
                EntryId = 355,
                EntryMedium = MediaMedium.Animeseries,
                EntryName = "Initial D First Stage",
                EntryType = MediaEntryType.Anime,
                Language = MediaLanguage.GerSub,
                TimeStamp = new DateTime(2016, 10, 18, 23, 15, 08)
            };
        }
    }
}