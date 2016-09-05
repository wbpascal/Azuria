using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Utilities.Properties
{
    /// <summary>
    ///     Represents a property that can be initialised.
    /// </summary>
    public interface IInitialisableProperty<T>
    {
        #region Properties

        /// <summary>
        ///     Gets a value whether the property was already initialised at least once.
        /// </summary>
        bool IsInitialisedOnce { get; }

        #endregion

        #region

        /// <summary>
        ///     Fetches a new value for the property without returning the new value.
        /// </summary>
        /// <returns>If the action was successful.</returns>
        [ItemNotNull]
        Task<ProxerResult> FetchObject();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        TaskAwaiter<ProxerResult<T>> GetAwaiter();

        /// <summary>
        ///     Gets a new value for the property independent of it being already initialised.
        /// </summary>
        /// <returns>If the action was successful and if it was, the value of this property.</returns>
        [ItemNotNull]
        Task<ProxerResult<T>> GetNewObject();

        /// <summary>
        ///     Gets a new value for the property independent of it being already initialised.
        /// </summary>
        /// <param name="onError">A value that is returned if the action was not successful.</param>
        /// <returns>
        ///     If the action was successful and if it was, the value of this property. If it was not then
        ///     <paramref name="onError" /> is returned.
        /// </returns>
        [ItemNotNull]
        Task<T> GetNewObject(T onError);

        /// <summary>
        ///     Initialises the property if it is not already.
        /// </summary>
        /// <returns>If the action was successful and if it was, the value of this property.</returns>
        [ItemNotNull]
        Task<ProxerResult<T>> GetObject();

        /// <summary>
        ///     Initialises the property if it is not already.
        /// </summary>
        /// <param name="onError">A value that is returned if the action was not successful.</param>
        /// <returns>
        ///     If the action was successful and if it was, the value of this property. If it was not then
        ///     <paramref name="onError" /> is returned.
        /// </returns>
        [ItemNotNull]
        Task<T> GetObject(T onError);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task<T> ThrowFirstOnNonSuccess();

        #endregion
    }
}