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
            this.IsInitialisedOnce = true;
        }

        #region Properties

        /// <summary>
        /// </summary>
        protected TOut InitialisedObject { get; set; }

        /// <summary>
        /// </summary>
        protected Func<TIn, Task<IProxerResult>> InitMethod { get; }

        /// <inheritdoc />
        public bool IsInitialisedOnce { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public async Task<IProxerResult> FetchObject(TIn param)
        {
            return await this.GetObject(param);
        }

        /// <inheritdoc />
        public async Task<IProxerResult<TOut>> GetNewObject(TIn param)
        {
            IProxerResult lInitialiseResult = await this.InitMethod.Invoke(param);
            return !lInitialiseResult.Success
                ? new ProxerResult<TOut>(lInitialiseResult.Exceptions)
                : new ProxerResult<TOut>(this.InitialisedObject);
        }

        /// <inheritdoc />
        public Task<TOut> GetNewObject(TIn param, TOut onError)
        {
            return this.GetNewObject(param).OnError(onError);
        }

        /// <inheritdoc />
        public async Task<IProxerResult<TOut>> GetObject(TIn param)
        {
            return this.IsInitialisedOnce
                ? new ProxerResult<TOut>(this.InitialisedObject)
                : await this.GetNewObject(param);
        }

        /// <inheritdoc />
        public Task<TOut> GetObject(TIn param, TOut onError)
        {
            return this.GetObject(param).OnError(onError);
        }

        /// <inheritdoc />
        public TOut GetObjectIfInitialised()
        {
            if (!this.IsInitialisedOnce) throw new NotInitialisedException();
            return this.InitialisedObject;
        }

        /// <inheritdoc />
        public TOut GetObjectIfInitialised(TOut ifNot)
        {
            return this.IsInitialisedOnce ? this.InitialisedObject : ifNot;
        }

        /// <summary>
        /// </summary>
        /// <param name="initialisedObject"></param>
        public void SetInitialisedObject(TOut initialisedObject)
        {
            this.InitialisedObject = initialisedObject;
            this.IsInitialisedOnce = true;
        }

        /// <inheritdoc />
        public Task<TOut> ThrowFirstOnNonSuccess(TIn param)
        {
            return this.GetObject(param).ThrowFirstForNonSuccess();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.GetObjectIfInitialised().ToString();
        }

        #endregion
    }
}