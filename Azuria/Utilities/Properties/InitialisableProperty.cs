using System;
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
        /// <summary>
        /// </summary>
        /// <param name="initMethod"></param>
        public InitialisableProperty(Func<Task<IProxerResult>> initMethod)
        {
            this.InitMethod = initMethod;
            this.IsInitialised = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="initMethod"></param>
        /// <param name="initialisationResult"></param>
        public InitialisableProperty(Func<Task<IProxerResult>> initMethod, T initialisationResult)
        {
            this.InitMethod = initMethod;
            this.InitialisedObject = initialisationResult;
            this.IsInitialised = true;
        }

        #region Properties

        /// <summary>
        /// </summary>
        protected T InitialisedObject { get; set; }

        /// <summary>
        /// </summary>
        protected Func<Task<IProxerResult>> InitMethod { get; }

        /// <inheritdoc />
        public bool IsInitialised { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public async Task<IProxerResult> FetchObject()
        {
            return await this.Get();
        }

        /// <inheritdoc />
        public async Task<IProxerResult<T>> Get()
        {
            return this.IsInitialised
                ? new ProxerResult<T>(this.InitialisedObject)
                : await this.GetNew();
        }

        /// <inheritdoc />
        public Task<T> Get(T onError)
        {
            return this.Get().OnError(onError);
        }

        /// <inheritdoc />
        public TaskAwaiter<IProxerResult<T>> GetAwaiter()
        {
            return this.Get().GetAwaiter();
        }

        /// <inheritdoc />
        public T GetIfInitialised()
        {
            if (!this.IsInitialised) throw new NotInitialisedException();
            return this.InitialisedObject;
        }

        /// <inheritdoc />
        public T GetIfInitialised(T ifNot)
        {
            return this.IsInitialised ? this.InitialisedObject : ifNot;
        }

        /// <inheritdoc />
        public async Task<IProxerResult<T>> GetNew()
        {
            IProxerResult lInitialiseResult = await this.InitMethod.Invoke();
            return !lInitialiseResult.Success
                ? new ProxerResult<T>(lInitialiseResult.Exceptions)
                : new ProxerResult<T>(this.InitialisedObject);
        }

        /// <inheritdoc />
        public Task<T> GetNew(T onError)
        {
            return this.GetNew().OnError(onError);
        }

        /// <summary>
        /// </summary>
        /// <param name="initialisedObject"></param>
        public void Set(T initialisedObject)
        {
            this.InitialisedObject = initialisedObject;
            this.IsInitialised = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="initialisedObject"></param>
        public void SetIfNotInitialised(T initialisedObject)
        {
            if (!this.IsInitialised) this.Set(initialisedObject);
        }

        /// <inheritdoc />
        public Task<T> ThrowFirstOnNonSuccess()
        {
            return this.Get().ThrowFirstForNonSuccess();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.GetIfInitialised().ToString();
        }

        #endregion
    }
}