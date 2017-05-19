using System;
using System.Collections.Generic;
using Autofac;
using Azuria.Authentication;
using Newtonsoft.Json;

namespace Azuria.Test.Core.Utility
{
    public static class TestingExtensions
    {
        #region Methods

        public static ProxerClientOptions WithTestingHttpClient(this ProxerClientOptions options)
        {
            return options.WithCustomHttpClient(
                context => ResponseSetup.GetTestingClient(options.ApiKey, context.Resolve<ILoginManager>())
            );
        }

        public static string GetExceptionInfo(this IEnumerable<Exception> exceptions)
        {
            return JsonConvert.SerializeObject(
                exceptions, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        #endregion
    }
}