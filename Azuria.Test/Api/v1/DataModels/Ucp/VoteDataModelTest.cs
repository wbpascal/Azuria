using System.Linq;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Ucp
{
    public class VoteDataModelTest : DataModelsTestBase<VoteDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["ucp_getvotes.json"];
            ProxerApiResponse<VoteDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(2, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        private static VoteDataModel BuildDataModel()
        {
            return new VoteDataModel
            {
                CommentContent = "Comment Content",
                CommentId = 581723414,
                EntryName = "Anime Name",
                Rating = 9,
                Type = "recommend",
                UserId = 163825,
                Username = "KutoSan",
                VoteId = 51726391
            };
        }
    }
}