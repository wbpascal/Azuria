using Azuria.Api.v1.DataModels.Media;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums.Media;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;
using Xunit;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    public class MediaRequestBuilderTest : RequestBuilderTestBase<MediaRequestBuilder>
    {
        [Fact]
        public void GetHeaderListTest()
        {
            IRequestBuilderWithResult<HeaderDataModel[]> lRequest = this.RequestBuilder.GetHeaderList();
            this.CheckUrl(lRequest, "media", "headerlist");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.False(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(HeaderStyle.Black)]
        [InlineData(HeaderStyle.Gray)]
        [InlineData(HeaderStyle.OldBlue)]
        [InlineData(HeaderStyle.Pantsu)]
        public void GetRandomHeaderTest(HeaderStyle style)
        {
            IRequestBuilderWithResult<HeaderDataModel> lRequest = this.RequestBuilder.GetRandomHeader(style);
            this.CheckUrl(lRequest, "media", "randomheader");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("style"));
            Assert.Equal(style.ToTypeString(), lRequest.GetParameters["style"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}