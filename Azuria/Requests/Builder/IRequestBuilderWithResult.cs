using System.Collections.Generic;
using Azuria.Api.v1.Converter;

namespace Azuria.Requests.Builder
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRequestBuilderWithResult<T> : IRequestBuilderBase
    {
        /// <summary>
        /// </summary>
        DataConverter<T> CustomDataConverter { get; }

        /// <summary>
        /// </summary>
        /// <param name="converter"></param>
        /// <returns></returns>
        IRequestBuilderWithResult<T> WithCustomDataConverter(DataConverter<T> converter);

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IRequestBuilderWithResult<T> WithGetParameter(string key, string value);

        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IRequestBuilderWithResult<T> WithGetParameter(IDictionary<string, string> parameter);

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IRequestBuilderWithResult<T> WithHeader(string key, string value);

        /// <summary>
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        IRequestBuilderWithResult<T> WithHeader(IDictionary<string, string> header);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IRequestBuilderWithResult<T> WithLoginCheck(bool check = true);

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IRequestBuilderWithResult<T> WithPostParameter(string key, string value);

        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IRequestBuilderWithResult<T> WithPostParameter(IEnumerable<KeyValuePair<string, string>> args);
    }
}