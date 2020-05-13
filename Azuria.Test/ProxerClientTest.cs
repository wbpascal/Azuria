using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture]
    public class ProxerClientTest
    {
        [Test]
        public void CreateNoOptionsTest()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);
            Assert.NotNull(lClient);
            Assert.IsNotEmpty(lClient.ApiKey);
            Assert.NotNull(lClient.Pipeline);
        }
    }
}