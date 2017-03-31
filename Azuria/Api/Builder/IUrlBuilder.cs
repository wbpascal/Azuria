using System.Collections.Generic;

namespace Azuria.Api.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUrlBuilder : IUrlBuilderBase
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IUrlBuilder WithGetParameter(string key, string value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IUrlBuilder WithGetParameter(IDictionary<string, string> parameter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IUrlBuilder WithPostParameter(string key, string value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IUrlBuilder WithPostParameter(IEnumerable<KeyValuePair<string, string>> args);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IUrlBuilderWithResult<T> WithResult<T>();

        #endregion
    }
}