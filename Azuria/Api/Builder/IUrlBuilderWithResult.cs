using System.Collections.Generic;
using Azuria.Api.v1.Converters;

namespace Azuria.Api.Builder
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUrlBuilderWithResult<T> : IUrlBuilderBase
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        DataConverter<T> CustomDataConverter { get; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="converter"></param>
        /// <returns></returns>
        IUrlBuilderWithResult<T> WithCustomDataConverter(DataConverter<T> converter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IUrlBuilderWithResult<T> WithGetParameter(string key, string value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IUrlBuilderWithResult<T> WithGetParameter(IDictionary<string, string> parameter);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IUrlBuilderWithResult<T> WithLoginCheck(bool check = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IUrlBuilderWithResult<T> WithPostParameter(string key, string value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IUrlBuilderWithResult<T> WithPostParameter(IEnumerable<KeyValuePair<string, string>> args);

        #endregion
    }
}