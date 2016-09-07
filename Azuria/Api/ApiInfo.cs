using System;
using Azuria.Api.v1;
using Azuria.Exceptions;
using Azuria.Security;
using Azuria.Web;

namespace Azuria.Api
{
    /// <summary>
    /// </summary>
    public static class ApiInfo
    {
        private static IHttpClient _httpClient;

        #region Properties

        internal static IHttpClient HttpClient
        {
            get
            {
                if (_httpClient == null) throw new ApiNotInitialisedException();
                return _httpClient;
            }
            set { _httpClient = value; }
        }

        internal static Func<ISecureContainer<char[]>> SecureContainerFactory { get; set; }

        #endregion

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
            SecureContainerFactory = input.SecureContainerFactory;
            HttpClient = input.CustomHttpClient;
            RequestHandler.Init(input.ApiKeyV1);
        }

        #endregion
    }
}