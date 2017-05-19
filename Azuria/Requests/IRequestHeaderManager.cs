using System.Collections.Generic;

namespace Azuria.Requests
{
    /// <summary>
    ///
    /// </summary>
    public interface IRequestHeaderManager
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetHeader();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        bool ContainsAuthenticationHeaders(Dictionary<string, string> header);
    }
}