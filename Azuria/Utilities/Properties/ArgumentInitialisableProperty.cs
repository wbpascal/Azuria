using System;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Utilities.Extensions;

namespace Azuria.Utilities.Properties
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public class ArgumentInitialisableProperty<TIn, TOut> : IArgumentInitialisableProperty<TIn, TOut>
    {
        /// <summary>
        /// </summary>
        /// <param name="initMethod"></param>
        public ArgumentInitialisableProperty(Func<TIn, Task<IProxerResult>> initMethod)
        {
            this.InitMethod = initMethod;
        }

        /// <summary>
        /// </summary>
        /// <param name="initMethod"></param>
        /// <param name="initialisationResult"></param>
        public ArgumentInitialisableProperty(Func<TIn, Task<IProxerResult>> initMethod, TOut initialisationResult)
            : this(initMethod)
        {
            this.InitialisedObject = initialisationResult;
            this.IsInitialised = true;
        }

        #region Properties

        /// <summary>
        /// </summary>
        protected TOut InitialisedObject { get; set; }

        /// <summary>
        /// </summary>
        protected Func<TIn, Task<IProxerResult>> InitMethod { get; }

        /// <inheritdoc />
        public bool IsInitialised { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public async Task<IProxerResult> FetchObject(TIn param)
        {
            return await this.Get(param).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IProxerResult<TOut>> Get(TIn param)
        {
            return this.IsInitialised
                ? new ProxerResult<TOut>(this.InitialisedObject)
                : await this.GetNew(param).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<TOut> Get(TIn param, TOut onError)
        {
            return this.Get(param).OnError(onError);
        }

        /// <inheritdoc />
        public TOut GetIfInitialised()
        {
            if (!this.IsInitialised) throw new NotInitialisedException();
            return this.InitialisedObject;
        }

        /// <inheritdoc />
        public TOut GetIfInitialised(TOut ifNot)
        {
            return this.IsInitialised ? this.InitialisedObject : ifNot;
        }

        /// <inheritdoc />
        public async Task<IProxerResult<TOut>> GetNew(TIn param)
        {
            IProxerResult lInitialiseResult = await this.InitMethod.Invoke(param).ConfigureAwait(false);
            return !lInitialiseResult.Success
                ? new ProxerResult<TOut>(lInitialiseResult.Exceptions)
                : new ProxerResult<TOut>(this.InitialisedObject);
        }

        /// <inheritdoc />
        public Task<TOut> GetNew(TIn param, TOut onError)
        {
            return this.GetNew(param).OnError(onError);
        }

        /// <summary>
        /// </summary>
        /// <param name="initialisedObject"></param>
        public void Set(TOut initialisedObject)
        {
            this.InitialisedObject = initialisedObject;
            this.IsInitialised = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="initialisedObject"></param>
        public void SetIfNotInitialised(TOut initialisedObject)
        {
            if (!this.IsInitialised) this.Set(initialisedObject);
        }

        /// <inheritdoc />
        public Task<TOut> ThrowFirstOnNonSuccess(TIn param)
        {
            return this.Get(param).ThrowFirstForNonSuccess<TOut>();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.GetIfInitialised().ToString();
        }

        #endregion
    }
}