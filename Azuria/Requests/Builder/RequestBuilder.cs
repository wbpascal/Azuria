using System;
using System.Collections.Generic;
using Azuria.Api.v1.Converters;

namespace Azuria.Requests.Builder
{
    /// <summary>
    /// </summary>
    public class RequestBuilder : RequestBuilderBase, IRequestBuilder
    {
        /// <summary>
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="client"></param>
        public RequestBuilder(Uri baseUri, IProxerClient client) : base(baseUri, client)
        {
        }

        /// <inheritdoc />
        public IRequestBuilder WithGetParameter(string key, string value)
        {
            this.AddGetParameter(key, value);
            return this;
        }

        /// <inheritdoc />
        public IRequestBuilder WithGetParameter(IDictionary<string, string> parameter)
        {
            this.AddGetParameter(parameter);
            return this;
        }

        /// <inheritdoc />
        public IRequestBuilder WithLoginCheck(bool check = true)
        {
            this.AddLoginCheck(check);
            return this;
        }

        /// <inheritdoc />
        public IRequestBuilder WithPostParameter(string key, string value)
        {
            this.AddPostArgument(key, value);
            return this;
        }

        /// <inheritdoc />
        public IRequestBuilder WithPostParameter(IEnumerable<KeyValuePair<string, string>> args)
        {
            this.AddPostArgument(args);
            return this;
        }

        /// <inheritdoc />
        public IRequestBuilderWithResult<T> WithResult<T>()
        {
            return new RequestBuilder<T>(this);
        }
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequestBuilder<T> : RequestBuilderBase, IRequestBuilderWithResult<T>
    {
        /// <summary>
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="client"></param>
        public RequestBuilder(Uri baseUri, IProxerClient client) : base(baseUri, client)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        public RequestBuilder(RequestBuilderBase builder) : base(builder)
        {
        }

        /// <inheritdoc />
        public DataConverter<T> CustomDataConverter { get; private set; }

        /// <inheritdoc />
        public IRequestBuilderWithResult<T> WithCustomDataConverter(DataConverter<T> converter)
        {
            this.CustomDataConverter = converter;
            return this;
        }

        /// <inheritdoc />
        public IRequestBuilderWithResult<T> WithGetParameter(string key, string value)
        {
            this.AddGetParameter(key, value);
            return this;
        }

        /// <inheritdoc />
        public IRequestBuilderWithResult<T> WithGetParameter(IDictionary<string, string> parameter)
        {
            this.AddGetParameter(parameter);
            return this;
        }

        /// <inheritdoc />
        public IRequestBuilderWithResult<T> WithLoginCheck(bool check = true)
        {
            this.AddLoginCheck(check);
            return this;
        }

        /// <inheritdoc />
        public IRequestBuilderWithResult<T> WithPostParameter(string key, string value)
        {
            this.AddPostArgument(key, value);
            return this;
        }

        /// <inheritdoc />
        public IRequestBuilderWithResult<T> WithPostParameter(IEnumerable<KeyValuePair<string, string>> args)
        {
            this.AddPostArgument(args);
            return this;
        }
    }
}