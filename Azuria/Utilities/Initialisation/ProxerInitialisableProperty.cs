using System;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Utilities.Initialisation
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProxerInitialisableProperty<T> : IInitialisableProperty<T>
    {
        [NotNull] private readonly Func<Task<ProxerResult>> _initMethod;
        [CanBeNull] private T _initialisedObject;

        /// <summary>
        /// </summary>
        /// <param name="initMethod"></param>
        public ProxerInitialisableProperty([NotNull] Func<Task<ProxerResult>> initMethod)
        {
            this._initMethod = initMethod;
            this.IsInitialisedOnce = false;
        }

        internal ProxerInitialisableProperty([NotNull] Func<Task<ProxerResult>> initMethod,
            [NotNull] T initialisationResult)
        {
            this._initMethod = initMethod;
            this._initialisedObject = initialisationResult;
            this.IsInitialisedOnce = true;
        }

        #region Geerbt

        /// <summary>
        ///     Mindestens einmal Initialisiert
        /// </summary>
        public bool IsInitialisedOnce { get; internal set; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        public async Task<ProxerResult<T>> GetObject()
        {
            if (this.IsInitialisedOnce && this._initialisedObject != null)
                return new ProxerResult<T>(this._initialisedObject);

            return await this.GetNewObject();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        public async Task<T> GetObject([NotNull] T onError)
        {
            return (await this.GetObject()).OnError(onError);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
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
        [ItemNotNull]
        [ContractAnnotation("null=>canbenull")]
        public async Task<T> GetNewObject(T onError)
        {
            return (await this.GetNewObject()).OnError(onError);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        public async Task<ProxerResult> FetchObject()
        {
            return await this.GetObject();
        }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <param name="ifNot"></param>
        /// <returns></returns>
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