using Azuria.Api.v1.DataModels.User;
using Azuria.ErrorHandling;
using Azuria.Helpers;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.User
{
    public class UserInfoDataModelTest : DataModelsTestBase<UserInfoDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["user_getuserinfo.json"];
            ProxerApiResponse<UserInfoDataModel> lResponse = this.Convert(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result);
        }

        private static UserInfoDataModel BuildDataModel()
        {
            return new UserInfoDataModel
            {
                AvatarId = "177103_TamLhZ.png",
                PointsAnime = 3912,
                PointsForum = 4,
                PointsInfo = 2,
                PointsManga = 624,
                PointsMisc = 200,
                PointsUploads = 97,
                StatusLastChanged = DateTimeHelpers.UnixTimeStampToDateTime(1432745294),
                StatusText = "StatusText",
                UserId = 1,
                Username = "Username"
            };
        }
    }
}