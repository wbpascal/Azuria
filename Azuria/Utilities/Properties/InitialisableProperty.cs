using System;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Utilities.Properties
{
    /// <summary>
    ///     Represents a property that can be initialised.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public class InitialisableProperty<T> : IInitialisableProperty<T>
    {
        [NotNull] private readonly Func<Task<ProxerResult>> _initMethod;
        [CanBeNull] private T _initialisedObject;

        /// <summary>
        ///     Initialises a new instance with a specified initialisation method.
        /// </summary>
        /// <param name="initMethod">The initialisation method.</param>
        public InitialisableProperty([NotNull] Func<Task<ProxerResult>> initMethod)
        {
            this._initMethod = initMethod;
            this.IsInitialisedOnce = false;
        }

        internal InitialisableProperty([NotNull] Func<Task<ProxerResult>> initMethod,
            [NotNull] T initialisationResult)
        {
            this._initMethod = initMethod;
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

        /// <summary>
        ///     Gets the current value of the property if the property was initialised at least once. If it was not the returns the
        ///     value specified in <paramref name="ifNot" />.
        /// </summary>
        /// <param name="ifNot">The value that is returned if the property was not initialised at least once.</param>
        /// <returns>The current value or the value of <paramref name="ifNot" />.</returns>
        [NotNull]
        public T GetObjectIfInitialised(T ifNot)
        {
            return this.IsInitialisedOnce && this._initialisedObject != null ? this._initialisedObject : ifNot;
        }

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

        #endregion
    }
}