using Azuria.Authentication;
using Azuria.Middleware.Pipeline;

namespace Azuria
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReadOnlyClientOptions
    {
        /// <summary>
        /// 
        /// </summary>
        char[] ApiKey { get; }

        /// <summary>
        /// 
        /// </summary>
        ILoginManager LoginManager { get; }

        /// <summary>
        /// 
        /// </summary>
        IPipeline Pipeline { get; }
    }
}