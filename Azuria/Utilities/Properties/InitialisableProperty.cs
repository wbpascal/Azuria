using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Utilities.Extensions;

namespace Azuria.Utilities.Properties
{
    /// <summary>
    /// Represents a property that can be initialised.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public class InitialisableProperty<T> : IInitialisableProperty<T>
    {
        private readonly Func<Task<IProxerResult>> _initMethod;
        private T _initialisedObject;

        /// <summary>
        /// </summary>
        /// <param name="initMethod"></param>
        public InitialisableProperty(Func<Task<IProxerResult>> initMethod)
        {
            this._initMethod = initMethod;
            this.IsInitialisedOnce = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="initMethod"></param>
        /// <param name="initialisationResult"></param>
        public InitialisableProperty(Func<Task<IProxerResult>> initMethod, T initialisationResult)
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
        public async Task<IProxerResult> FetchObject()
        {
            return await this.GetObject();
        }

        /// <inheritdoc />
        public TaskAwaiter<IProxerResult<T>> GetAwaiter()
        {
            return this.GetObject().GetAwaiter();
        }

        /// <inheritdoc />
        public async Task<IProxerResult<T>> GetNewObject()
        {
            IProxerResult lInitialiseResult = await this._initMethod.Invoke();
            return !lInitialiseResult.Success
                ? new ProxerResult<T>(lInitialiseResult.Exceptions)
                : new ProxerResult<T>(this._initialisedObject);
        }

        /// <inheritdoc />
        public Task<T> GetNewObject(T onError)
        {
            return this.GetNewObject().OnError(onError);
        }

        /// <inheritdoc />
        public async Task<IProxerResult<T>> GetObject()
        {
            if (this.IsInitialisedOnce)
                return new ProxerResult<T>(this._initialisedObject);

            return await this.GetNewObject();
        }

        /// <inheritdoc />
        public async Task<T> GetObject(T onError)
        {
            return (await this.GetObject()).OnError(onError);
        }

        /// <inheritdoc />
        public T GetObjectIfInitialised()
        {
            if (!this.IsInitialisedOnce) throw new NotInitialisedException();
            return this._initialisedObject;
        }

        /// <inheritdoc />
        public T GetObjectIfInitialised(T ifNot)
        {
            return this.IsInitialisedOnce ? this._initialisedObject : ifNot;
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
            IProxerResult<T> lResult = await this.GetObject();
            if (!lResult.Success)
                throw lResult.Exceptions.FirstOrDefault() ?? new Exception();

            return lResult.Result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.GetObjectIfInitialised().ToString();
        }

        #endregion
    }
}