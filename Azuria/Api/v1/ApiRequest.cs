using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.Converters;

namespace Azuria.Api.v1
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiRequest<T> : ApiRequest
    {
        /// <inheritdoc />
        protected ApiRequest(Uri address) : base(address)
        {
        }

        #region Properties

        /// <summary>
        /// </summary>
        public DataConverter<T> CustomDataConverter { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public new static ApiRequest<T> Create(Uri address)
        {
            return new ApiRequest<T>(address);
        }

        /// <inheritdoc />
        public new ApiRequest<T> WithCheckLogin(bool checkLogin)
        {
            this.CheckLogin = checkLogin;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="customConverter"></param>
        /// <returns></returns>
        public ApiRequest<T> WithCustomDataConverter(DataConverter<T> customConverter)
        {
            this.CustomDataConverter = customConverter;
            return this;
        }

        /// <inheritdoc />
        public new ApiRequest<T> WithPostArgument(string key, string value)
        {
            List<KeyValuePair<string, string>> lPostArgs = this.PostArguments.ToList();
            lPostArgs.Add(new KeyValuePair<string, string>(key, value));
            this.PostArguments = lPostArgs;
            return this;
        }

        /// <inheritdoc />
        public new ApiRequest<T> WithPostArguments(IEnumerable<KeyValuePair<string, string>> postArgs)
        {
            this.PostArguments = postArgs;
            return this;
        }

        /// <inheritdoc />
        public new ApiRequest<T> WithSenpai(Senpai senpai)
        {
            this.Senpai = senpai;
            return this;
        }

        #endregion
    }

    /// <summary>
    /// </summary>
    public class ApiRequest
    {
        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        protected ApiRequest(Uri address)
        {
            this.Address = address;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public Uri Address { get; set; }

        /// <summary>
        /// </summary>
        public bool CheckLogin { get; set; }

        /// <summary>
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> PostArguments { get; set; } =
            new Dictionary<string, string>();

        /// <summary>
        /// </summary>
        public Senpai Senpai { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static ApiRequest Create(Uri address)
        {
            return new ApiRequest(address);
        }

        /// <summary>
        /// </summary>
        /// <param name="checkLogin"></param>
        /// <returns></returns>
        public ApiRequest WithCheckLogin(bool checkLogin)
        {
            this.CheckLogin = checkLogin;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public ApiRequest WithPostArgument(string key, string value)
        {
            List<KeyValuePair<string, string>> lPostArgs = this.PostArguments.ToList();
            lPostArgs.Add(new KeyValuePair<string, string>(key, value));
            this.PostArguments = lPostArgs;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public ApiRequest WithPostArguments(IEnumerable<KeyValuePair<string, string>> postArgs)
        {
            this.PostArguments = postArgs;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public ApiRequest WithSenpai(Senpai senpai)
        {
            this.Senpai = senpai;
            return this;
        }

        #endregion
    }
}