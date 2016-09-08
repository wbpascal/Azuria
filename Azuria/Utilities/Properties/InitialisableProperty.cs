using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Utilities.Properties
{
    /// <summary>
    ///     Represents a property that can be initialised.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public class InitialisableProperty<T> : IInitialisableProperty<T>
    {
        private readonly Func<Task<ProxerResult>> _initMethod;
        private T _initialisedObject;

        /// <summary>
        /// </summary>
        /// <param name="initMethod"></param>
        public InitialisableProperty(Func<Task<ProxerResult>> initMethod)
        {
            this._initMethod = initMethod;
            this.IsInitialisedOnce = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="initMethod"></param>
        /// <param name="initialisationResult"></param>
        public InitialisableProperty(Func<Task<ProxerResult>> initMethod, T initialisationResult)
        {
            this._initMethod = initMethod;
            this._initialisedObject = initialisationResult;
            this.IsInitialisedOnce = true;
        }

        #region Properties

        /// <inheritdoc />
        public bool IsInitialisedOnce { get; internal set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public async Task<ProxerResult> FetchObject()
        {
            return await this.GetObject();
        }

        /// <inheritdoc />
        public TaskAwaiter<ProxerResult<T>> GetAwaiter()
        {
            return this.GetObject().GetAwaiter();
        }

        /// <inheritdoc />
        public async Task<ProxerResult<T>> GetNewObject()
        {
            ProxerResult lInitialiseResult = await this._initMethod.Invoke();
            if (!lInitialiseResult.Success || (this._initialisedObject == null))
                return new ProxerResult<T>(lInitialiseResult.Exceptions);

            return new ProxerResult<T>(this._initialisedObject);
        }

        /// <inheritdoc />
        public async Task<T> GetNewObject(T onError)
        {
            return (await this.GetNewObject()).OnError(onError);
        }

        /// <inheritdoc />
        public async Task<ProxerResult<T>> GetObject()
        {
            if (this.IsInitialisedOnce && (this._initialisedObject != null))
                return new ProxerResult<T>(this._initialisedObject);

            return await this.GetNewObject();
        }

        /// <inheritdoc />
        public async Task<T> GetObject(T onError)
        {
            return (await this.GetObject()).OnError(onError);
        }

        /// <inheritdoc />
        public T GetObjectIfInitialised(T ifNot)
        {
            return this.IsInitialisedOnce && (this._initialisedObject != null) ? this._initialisedObject : ifNot;
        }

        /// <summary>
        /// </summary>
        /// <param name="initialisedObject"></param>
        public void SetInitialisedObject(T initialisedObject)
        {
            this._initialisedObject = initialisedObject;
            this.IsInitialisedOnce = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="initialisedObject"></param>
        /// <returns></returns>
        public T SetInitialisedObjectAndReturn(T initialisedObject)
        {
            this._initialisedObject = initialisedObject;
            this.IsInitialisedOnce = true;
            return initialisedObject;
        }

        /// <inheritdoc />
        public async Task<T> ThrowFirstOnNonSuccess()
        {
            ProxerResult<T> lResult = await this.GetObject();
            if (!lResult.Success || (lResult.Result == null))
                throw lResult.Exceptions.FirstOrDefault() ?? new Exception();

            return lResult.Result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.GetObjectIfInitialised(default(T)).ToString();
        }

        #endregion
    }
}