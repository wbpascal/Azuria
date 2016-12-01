using System;
using System.Collections.Generic;
using Azuria.Api.v1.Converters;

namespace Azuria.Api.v1
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiRequest<T> : ApiRequest
    {
        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        public ApiRequest(Uri address) : base(address)
        {
        }

        #region Properties

        /// <summary>
        /// </summary>
        public DataConverter<T> CustomDataConverter { get; set; }

        #endregion
    }

    /// <summary>
    /// </summary>
    public class ApiRequest
    {
        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        public ApiRequest(Uri address)
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
    }
}