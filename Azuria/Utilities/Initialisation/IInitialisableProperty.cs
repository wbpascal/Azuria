using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Utilities.Initialisation
{
    /// <summary>
    /// </summary>
    public interface IInitialisableProperty<T>
    {
        #region Properties

        /// <summary>
        ///     Mindestens einmal Initialisiert
        /// </summary>
        bool IsInitialisedOnce { get; }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        Task<ProxerResult> FetchObject();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        Task<ProxerResult<T>> GetNewObject();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        Task<T> GetNewObject(T onError);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        Task<ProxerResult<T>> GetObject();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        Task<T> GetObject(T onError);

        #endregion
    }
}