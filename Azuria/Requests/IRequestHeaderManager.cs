using System.Collections.Generic;

namespace Azuria.Requests
{
    /// <summary>
    /// </summary>
    public interface IRequestHeaderManager
    {
        /// <summary>
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        bool ContainsAuthenticationHeaders(IDictionary<string, string> header);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetHeader();
    }
}