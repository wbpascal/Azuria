using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.Converters;
using Azuria.Helpers.Extensions;

namespace Azuria.Api.v1
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiRequest<T> : ApiRequest
    {
        /// <inheritdoc />
        protected ApiRequest(Uri baseAddress) : base(baseAddress)
        {
        }

        #region Properties

        /// <summary>
        /// </summary>
        public DataConverter<T> CustomDataConverter { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public new static ApiRequest<T> Create(Uri baseAddress)
        {
            return new ApiRequest<T>(baseAddress);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public new ApiRequest<T> WithGetParameter(string key, string value)
        {
            base.WithGetParameter(key, value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getArgs"></param>
        /// <returns></returns>
        public new ApiRequest<T> WithGetParameters(IDictionary<string, string> getArgs)
        {
            base.WithGetParameters(getArgs);
            return this;
        }

        /// <inheritdoc />
        public new ApiRequest<T> WithLoginCheck(bool checkLogin)
        {
            base.WithLoginCheck(checkLogin);
            return this;
        }

        /// <inheritdoc />
        public new ApiRequest<T> WithPostParameter(string key, string value)
        {
            base.WithPostParameter(key, value);
            return this;
        }

        /// <inheritdoc />
        public new ApiRequest<T> WithPostParameters(IEnumerable<KeyValuePair<string, string>> postArgs)
        {
            base.WithPostParameters(postArgs);
            return this;
        }

        #endregion
    }

    /// <summary>
    /// </summary>
    public class ApiRequest
    {
        private List<KeyValuePair<string, string>> _postArguments = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// </summary>
        /// <param name="baseAddress"></param>
        protected ApiRequest(Uri baseAddress)
        {
            this.BaseAddress = baseAddress;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public Uri BaseAddress { get; set; }

        /// <summary>
        /// </summary>
        public bool CheckLogin { get; set; }

        /// <summary>
        /// </summary>
        public Uri FullAddress => this.BuildAddress();

        /// <summary>
        /// </summary>
        public IDictionary<string, string> GetParameter { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> PostArguments
        {
            get { return this._postArguments; }
            set { this._postArguments = value.ToList(); }
        }

        #endregion

        #region Methods

        private Uri BuildAddress()
        {
            UriBuilder lUriBuilder = new UriBuilder(this.BaseAddress);
            string lQuery = lUriBuilder.Query;
            foreach (KeyValuePair<string, string> pair in this.GetParameter)
                lQuery += $"&{pair.Key}={pair.Value}";
            if (!string.IsNullOrEmpty(lQuery)) lUriBuilder.Query = lQuery.Remove(0, 1);
            return lUriBuilder.Uri;
        }

        /// <summary>
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <returns></returns>
        public static ApiRequest Create(Uri baseAddress)
        {
            return new ApiRequest(baseAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ApiRequest WithGetParameter(string key, string value)
        {
            this.GetParameter[key] = value;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getArgs"></param>
        /// <returns></returns>
        public ApiRequest WithGetParameters(IDictionary<string, string> getArgs)
        {
            this.GetParameter.AddOrUpdateRange(getArgs);
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="checkLogin"></param>
        /// <returns></returns>
        public ApiRequest WithLoginCheck(bool checkLogin)
        {
            this.CheckLogin = checkLogin;
            return this;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public ApiRequest WithPostParameter(string key, string value)
        {
            this._postArguments.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public ApiRequest WithPostParameters(IEnumerable<KeyValuePair<string, string>> postArgs)
        {
            this._postArguments.AddRange(postArgs);
            return this;
        }

        #endregion
    }
}