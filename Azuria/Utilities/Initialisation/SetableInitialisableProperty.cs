using System;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Utilities.Initialisation
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SetableInitialisableProperty<T> : IInitialisableProperty<T>
    {
        [NotNull] private readonly Func<Task<ProxerResult>> _initMethod;

        [NotNull] private readonly Func<T, Task<ProxerResult>> _setMethod;

        [CanBeNull] private T _initialisedObject;

        /// <summary>
        /// </summary>
        /// <param name="initMethod"></param>
        /// <param name="setMethod"></param>
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
        ///     Mindestens einmal Initialisiert
        /// </summary>
        public bool IsInitialisedOnce { get; private set; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<ProxerResult> FetchObject()
        {
            return await this.GetObject();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<ProxerResult<T>> GetNewObject()
        {
            ProxerResult lInitialiseResult = await this._initMethod.Invoke();
            if (!lInitialiseResult.Success || this._initialisedObject == null)
                return new ProxerResult<T>(lInitialiseResult.Exceptions);

            return new ProxerResult<T>(this._initialisedObject);
        }

        /// <summary>
        /// </summary>
        /// <param name="onError"></param>
        /// <returns></returns>
        [ContractAnnotation("null=>canbenull")]
        public async Task<T> GetNewObject(T onError)
        {
            return (await this.GetNewObject()).OnError(onError);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<ProxerResult<T>> GetObject()
        {
            if (this.IsInitialisedOnce && this._initialisedObject != null)
                return new ProxerResult<T>(this._initialisedObject);

            return await this.GetNewObject();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<T> GetObject(T onError)
        {
            return (await this.GetObject()).OnError(onError);
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
        /// </summary>
        /// <param name="newObject"></param>
        /// <returns></returns>
        [ItemNotNull]
        public async Task<ProxerResult<T>> SetObject([NotNull] T newObject)
        {
            ProxerResult lInvokeResult = await this._setMethod.Invoke(newObject);
            if (!lInvokeResult.Success) return new ProxerResult<T>(lInvokeResult.Exceptions);
            this._initialisedObject = newObject;
            return new ProxerResult<T>(this._initialisedObject);
        }

        #endregion
    }
}