using Azuria.Enums;
using Azuria.ErrorHandling;
using NUnit.Framework;

namespace Azuria.Test.ErrorHandling
{
    [TestFixture]
    public class ProxerApiResponseTest
    {
        [Test]
        public void GetErrorCode_CorrectlyConvertsEnumValuesTest([Values] ErrorCode code)
        {
            var proxerApiResponse = new ProxerApiResponse() {ErrorCode = (int) code};
            Assert.AreEqual(code, proxerApiResponse.GetErrorCode());
        }

        [Test]
        public void GetErrorCode_ReturnsUnknownForOtherValuesTest()
        {
            var proxerApiResponse = new ProxerApiResponse {ErrorCode = -1};
            Assert.AreEqual(ErrorCode.Unknown, proxerApiResponse.GetErrorCode());

            proxerApiResponse = new ProxerApiResponse {ErrorCode = 5};
            Assert.AreEqual(ErrorCode.Unknown, proxerApiResponse.GetErrorCode());

            proxerApiResponse = new ProxerApiResponse {ErrorCode = 42000};
            Assert.AreEqual(ErrorCode.Unknown, proxerApiResponse.GetErrorCode());
        }
    }
}