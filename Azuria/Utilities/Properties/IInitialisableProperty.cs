using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Utilities.Properties
{
    /// <summary>
    /// Represents a property that can be initialised.
    /// </summary>
    public interface IInitialisableProperty<T>
    {
        #region Properties

        /// <summary>
        /// Gets a value whether the property was already initialised at least once.
        /// </summary>
        bool IsInitialised { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches a new value for the property without returning the new value.
        /// </summary>
        /// <returns>If the action was successful.</returns>
        Task<IProxerResult> FetchObject();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        TaskAwaiter<IProxerResult<T>> GetAwaiter();

        /// <summary>
        /// Gets a new value for the property independent of it being already initialised.
        /// </summary>
        /// <returns>If the action was successful and if it was, the value of this property.</returns>
        Task<IProxerResult<T>> GetNew();

        /// <summary>
        /// Gets a new value for the property independent of it being already initialised.
        /// </summary>
        /// <param name="onError">A value that is returned if the action was not successful.</param>
        /// <returns>
        /// If the action was successful and if it was, the value of this property. If it was not then
        /// <paramref name="onError" /> is returned.
        /// </returns>
        Task<T> GetNew(T onError);

        /// <summary>
        /// Initialises the property if it is not already.
        /// </summary>
        /// <returns>If the action was successful and if it was, the value of this property.</returns>
        Task<IProxerResult<T>> Get();

        /// <summary>
        /// Initialises the property if it is not already.
        /// </summary>
        /// <param name="onError">A value that is returned if the action was not successful.</param>
        /// <returns>
        /// If the action was successful and if it was, the value of this property. If it was not then
        /// <paramref name="onError" /> is returned.
        /// </returns>
        Task<T> Get(T onError);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        T GetIfInitialised();

        /// <summary>
        /// </summary>
        /// <param name="ifNot"></param>
        /// <returns></returns>
        T GetIfInitialised(T ifNot);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task<T> ThrowFirstOnNonSuccess();

        #endregion
    }
}