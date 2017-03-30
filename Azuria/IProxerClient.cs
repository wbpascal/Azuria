using Autofac;

namespace Azuria
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxerClient
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        char[] ApiKey { get; }

        /// <summary>
        /// 
        /// </summary>
        IContainer Container { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsAuthenticated { get; set; }

        #endregion
    }
}