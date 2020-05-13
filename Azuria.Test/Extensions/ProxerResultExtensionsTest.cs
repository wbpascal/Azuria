using System;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Helpers.Extensions;
using NUnit.Framework;

namespace Azuria.Test.Extensions
{
    public class ProxerResultExtensionsTest
    {
        [Test]
        public void OnErrorTest()
        {
            var lToken = new object();
            Assert.AreSame(lToken, new ProxerResult<object>(new Exception()).OnError(lToken));
            Assert.AreSame(lToken, new ProxerResult<object>(new Exception()).OnError(() => lToken));
        }

        [Test]
        public async Task OnErrorAsyncTest()
        {
            var lToken = new object();
            Task<ProxerResult<object>> lTask = Task.Run(() => new ProxerResult<object>(new Exception()));
            Assert.AreSame(lToken, await lTask.OnError(lToken));
            lTask = Task.Run(() => new ProxerResult<object>(new Exception()));
            Assert.AreSame(lToken, await lTask.OnError(() => Task.FromResult(lToken)));
        }
    }
}