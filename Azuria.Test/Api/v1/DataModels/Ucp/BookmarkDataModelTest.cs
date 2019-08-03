using System.Linq;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Ucp
{
    public class BookmarkDataModelTest : DataModelsTestBase<BookmarkDataModelBase>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["ucp_getreminder.json"];
            ProxerApiResponse<BookmarkDataModelBase[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(3, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        private static BookmarkDataModelBase BuildDataModel()
        {
            return new BookmarkDataModelBase
            {
                BookmarkId = 45583336,
                ContentIndex = 320,
                EntryId = 4188,
                EntryMedium = MediaMedium.Mangaseries,
                EntryName = "Tower of God",
                EntryType = MediaEntryType.Manga,
                Language = MediaLanguage.English,
                Status = MediaStatus.Airing
            };
        }
    }
}