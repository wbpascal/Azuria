using System;
using System.Collections.Generic;
using Azuria.Api.v1.Converters;

namespace Azuria.Api.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public class UrlBuilder : UrlBuilderBase, IUrlBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="client"></param>
        public UrlBuilder(Uri baseUri, IProxerClient client) : base(baseUri, client)
        {
        }

        #region Methods

        /// <inheritdoc />
        public IUrlBuilder WithGetParameter(string key, string value)
        {
            this.AddGetParameter(key, value);
            return this;
        }

        /// <inheritdoc />
        public IUrlBuilder WithGetParameter(IDictionary<string, string> parameter)
        {
            this.AddGetParameter(parameter);
            return this;
        }

        /// <inheritdoc />
        public IUrlBuilder WithPostParameter(string key, string value)
        {
            this.AddPostArgument(key, value);
            return this;
        }

        /// <inheritdoc />
        public IUrlBuilder WithPostParameter(IEnumerable<KeyValuePair<string, string>> args)
        {
            this.AddPostArgument(args);
            return this;
        }

        /// <inheritdoc />
        public IUrlBuilderWithResult<T> WithResult<T>()
        {
            return new UrlBuilder<T>(this);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UrlBuilder<T> : UrlBuilderBase, IUrlBuilderWithResult<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="client"></param>
        public UrlBuilder(Uri baseUri, IProxerClient client) : base(baseUri, client)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public UrlBuilder(UrlBuilderBase builder) : base(builder)
        {
        }

        #region Properties

        /// <inheritdoc />
        public DataConverter<T> CustomDataConverter { get; private set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IUrlBuilderWithResult<T> WithCustomDataConverter(DataConverter<T> converter)
        {
            this.CustomDataConverter = converter;
            return this;
        }

        /// <inheritdoc />
        public IUrlBuilderWithResult<T> WithGetParameter(string key, string value)
        {
            this.AddGetParameter(key, value);
            return this;
        }

        /// <inheritdoc />
        public IUrlBuilderWithResult<T> WithGetParameter(IDictionary<string, string> parameter)
        {
            this.AddGetParameter(parameter);
            return this;
        }

        /// <inheritdoc />
        public IUrlBuilderWithResult<T> WithPostParameter(string key, string value)
        {
            this.AddPostArgument(key, value);
            return this;
        }

        /// <inheritdoc />
        public IUrlBuilderWithResult<T> WithPostParameter(IEnumerable<KeyValuePair<string, string>> args)
        {
            this.AddPostArgument(args);
            return this;
        }

        #endregion
    }
}