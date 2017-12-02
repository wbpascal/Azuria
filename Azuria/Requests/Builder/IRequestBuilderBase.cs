using System;
using System.Collections.Generic;

namespace Azuria.Requests.Builder
{
    /// <summary>
    /// </summary>
    public interface IRequestBuilderBase
    {
        /// <summary>
        /// </summary>
        bool CheckLogin { get; }

        /// <summary>
        /// </summary>
        IProxerClient Client { get; }

        /// <summary>
        /// </summary>
        IDictionary<string, string> GetParameters { get; }

        /// <summary>
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> PostParameter { get; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Uri BuildUri();
    }
}