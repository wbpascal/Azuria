using System.Collections.Generic;

namespace Azuria.Requests.Builder
{
    /// <summary>
    /// </summary>
    public interface IRequestBuilder : IRequestBuilderBase
    {
        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IRequestBuilder WithGetParameter(string key, string value);

        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IRequestBuilder WithGetParameter(IDictionary<string, string> parameter);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IRequestBuilder WithLoginCheck(bool check = true);

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IRequestBuilder WithPostParameter(string key, string value);

        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IRequestBuilder WithPostParameter(IEnumerable<KeyValuePair<string, string>> args);

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRequestBuilderWithResult<T> WithResult<T>();
    }
}