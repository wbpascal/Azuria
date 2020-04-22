using System.Threading;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Middleware;
using Azuria.Requests.Builder;

namespace Azuria.Test.Middleware
{
    class TestMiddleware : IMiddleware
    {
        private readonly object _obj;

        public TestMiddleware(object obj)
        {
            this._obj = obj;
        }

        public Task<IProxerResult> Invoke(IRequestBuilder request, MiddlewareAction next, CancellationToken cancellationToken = default)
        {
            return Task.FromResult((IProxerResult) new ProxerApiResponse {Success = true, Message = "TestMiddleware"});
        }

        public Task<IProxerResult<T>> InvokeWithResult<T>(IRequestBuilderWithResult<T> request, MiddlewareAction<T> next,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult((IProxerResult<T>) new ProxerApiResponse<T> {Success = true, Result = (T) this._obj, Message = "TestMiddleware"});
        }
    }
}
