using System.Collections.Generic;

namespace Azuria.Api.v1.Input
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInputDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> Build();
    }
}