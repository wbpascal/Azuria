using System;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Utilities.Properties
{
    /// <summary>
    ///     Represents a property that can be initialised and set.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public class SetableInitialisableProperty<T> : IInitialisableProperty<T>
    {
        [NotNull] private readonly Func<Task<ProxerResult>> _initMethod;

        [NotNull] private readonly Func<T, Task<ProxerResult>> _setMethod;

        [CanBeNull] private T _initialisedObject;

        /// <summary>
        ///     Initialises a new instance with a specified initialisation method and a set method.
        /// </summary>
        /// <param name="initMethod">The initialisation method.</param>
        /// <param name="setMethod">The set method.</param>
        public SetableInitialisableProperty([NotNull] Func<Task<ProxerResult>> initMethod,
            [NotNull] Func<T, Task<ProxerResult>> setMethod)
        {
            this._initMethod = initMethod;
            this._setMethod = setMethod;
            this.IsInitialisedOnce = false;
        }

        internal SetableInitialisableProperty([NotNull] Func<Task<ProxerResult>> initMethod,
            [NotNull] Func<T, Task<ProxerResult>> setMethod,
            [NotNull] T initialisationResult)
        {
            this._initMethod = initMethod;
            this._setMethod = setMethod;
            this._initialisedObject = initialisationResult;
            this.IsInitialisedOnce = true;
        }

        #region Geerbt

        /// <summary>
        ///     Gets a value whether the property was already initialised at least once.
        /// </summary>
        public bool IsInitialisedOnce { get; internal set; }

        /// <summary>
        ///     Initialises the property if it is not already.
        /// </summary>
        /// <returns>If the action was successful and if it was, the value of this property.</returns>
        public async Task<ProxerResult<T>> GetObject()
        {
            if (this.IsInitialisedOnce && this._initialisedObject != null)
                return new ProxerResult<T>(this._initialisedObject);

            return await this.GetNewObject();
        }

        /// <summary>
        ///     Initialises the property if it is not already.
        /// </summary>
        /// <param name="onError">A value that is returned if the action was not successful.</param>
        /// <returns>
        ///     If the action was successful and if it was, the value of this property. If it was not then
        ///     <paramref name="onError" /> is returned.
        /// </returns>
        public async Task<T> GetObject([NotNull] T onError)
        {
            return (await this.GetObject()).OnError(onError);
        }

        /// <summary>
        ///     Gets a new value for the property independent of it being already initialised.
        /// </summary>
        /// <returns>If the action was successful and if it was, the value of this property.</returns>
        public async Task<ProxerResult<T>> GetNewObject()
        {
            ProxerResult lInitialiseResult = await this._initMethod.Invoke();
            if (!lInitialiseResult.Success || this._initialisedObject == null)
                return new ProxerResult<T>(lInitialiseResult.Exceptions);

            return new ProxerResult<T>(this._initialisedObject);
        }

        /// <summary>
        ///     Gets a new value for the property independent of it being already initialised.
        /// </summary>
        /// <param name="onError">A value that is returned if the action was not successful.</param>
        /// <returns>
        ///     If the action was successful and if it was, the value of this property. If it was not then
        ///     <paramref name="onError" /> is returned.
        /// </returns>
        [ContractAnnotation("null=>canbenull")]
        public async Task<T> GetNewObject(T onError)
        {
            return (await this.GetNewObject()).OnError(onError);
        }

        /// <summary>
        ///     Fetches a new value for the property without returning the new value.
        /// </summary>
        /// <returns>If the action was successful.</returns>
        public async Task<ProxerResult> FetchObject()
        {
            return await this.GetObject();
        }

        #endregion

        #region

        internal void SetInitialisedObject(T initialisedObject)
        {
            this._initialisedObject = initialisedObject;
            this.IsInitialisedOnce = true;
        }

        [ContractAnnotation("null=>null")]
        internal T SetInitialisedObjectAndReturn(T initialisedObject)
        {
            this._initialisedObject = initialisedObject;
            this.IsInitialisedOnce = true;
            return initialisedObject;
        }

        /// <summary>
        ///     Sets the value of the property to the value of <paramref name="newValue" />.
        /// </summary>
        /// <param name="newValue">The new value of the property.</param>
        /// <returns>If the action was successful and if it was, the current value of the property.</returns>
        [ItemNotNull]
        public async Task<ProxerResult<T>> SetObject([NotNull] T newValue)
        {
            ProxerResult lInvokeResult = await this._setMethod.Invoke(newValue);
            if (!lInvokeResult.Success) return new ProxerResult<T>(lInvokeResult.Exceptions);
            this._initialisedObject = newValue;
            return new ProxerResult<T>(this._initialisedObject);
        }

        #endregion
    }
}