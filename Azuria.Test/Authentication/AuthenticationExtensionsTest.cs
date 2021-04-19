using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.Input.User;
using Azuria.Authentication;
using Azuria.ErrorHandling;
using Azuria.Test.Core.Helpers;
using Moq;
using NUnit.Framework;

namespace Azuria.Test.Authentication
{
    [TestFixture]
    public class AuthenticationExtensionsTest
    {
        [Test]
        public void TryFindLoginManager_FindsLoginManager()
        {
            var loginManager = new DefaultLoginManager();
            IProxerClient client = ProxerClient.Create(new char[32], options => options.WithCustomLoginManager(loginManager));

            ILoginManager foundLoginManager = client.TryFindLoginManager();
            Assert.IsNotNull(foundLoginManager);
            Assert.AreSame(loginManager, foundLoginManager);
        }

        [Test]
        public void TryFindLoginManager_ReturnsNullWhenNoLoginManagerFound()
        {
            IProxerClient client = ProxerClient.Create(new char[32]);

            ILoginManager foundLoginManager = client.TryFindLoginManager();
            Assert.IsNull(foundLoginManager);
        }
    }
}