using System;
using Azuria.Api.Services;
using Azuria.Api.v1;

namespace Azuria.Api
{
    /// <summary>
    /// </summary>
    public static class ApiInfo
    {
        #region Methods

        /// <summary>
        /// </summary>
        public static void Init(Action<ApiInfoInput> inputFactory)
        {
            ApiInfoInput lInput = new ApiInfoInput();
            inputFactory.Invoke(lInput);
            Init(lInput);
        }

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        public static void Init(ApiInfoInput input)
        {
            RequestHandler.Init(input.ApiKeyV1);
            HttpClientService.Init(input.HttpClientFactory);
        }

        #endregion
    }
}