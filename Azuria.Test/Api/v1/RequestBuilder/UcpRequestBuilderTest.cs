using Azuria.Api.v1.RequestBuilder;
using Xunit;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    public class UcpRequestBuilderTest : RequestBuilderTestBase<UcpRequestBuilder>
    {
        [Fact]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}