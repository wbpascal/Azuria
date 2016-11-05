﻿using System;
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
            this.IsInitialisedOnce = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="initMethod"></param>
        /// <param name="initialisationResult"></param>
        public InitialisableProperty(Func<Task<IProxerResult>> initMethod, T initialisationResult)
        {
            this.InitMethod = initMethod;
            this.InitialisedObject = initialisationResult;
            this.IsInitialisedOnce = true;
        }

        #region Properties

        /// <summary>
        /// </summary>
        protected T InitialisedObject { get; set; }

        /// <summary>
        /// </summary>
        protected Func<Task<IProxerResult>> InitMethod { get; }

        /// <inheritdoc />
        public bool IsInitialisedOnce { get; set; }

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
            IProxerResult lInitialiseResult = await this.InitMethod.Invoke();
            return !lInitialiseResult.Success
                ? new ProxerResult<T>(lInitialiseResult.Exceptions)
                : new ProxerResult<T>(this.InitialisedObject);
        }

        /// <inheritdoc />
        public Task<T> GetNewObject(T onError)
        {
            return this.GetNewObject().OnError(onError);
        }

        /// <inheritdoc />
        public async Task<IProxerResult<T>> GetObject()
        {
            return this.IsInitialisedOnce
                ? new ProxerResult<T>(this.InitialisedObject)
                : await this.GetNewObject();
        }

        /// <inheritdoc />
        public Task<T> GetObject(T onError)
        {
            return this.GetObject().OnError(onError);
        }

        /// <inheritdoc />
        public T GetObjectIfInitialised()
        {
            if (!this.IsInitialisedOnce) throw new NotInitialisedException();
            return this.InitialisedObject;
        }

        /// <inheritdoc />
        public T GetObjectIfInitialised(T ifNot)
        {
            return this.IsInitialisedOnce ? this.InitialisedObject : ifNot;
        }

        /// <summary>
        /// </summary>
        /// <param name="initialisedObject"></param>
        public void SetInitialisedObject(T initialisedObject)
        {
            this.InitialisedObject = initialisedObject;
            this.IsInitialisedOnce = true;
        }

        /// <inheritdoc />
        public Task<T> ThrowFirstOnNonSuccess()
        {
            return this.GetObject().ThrowFirstForNonSuccess();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.GetObjectIfInitialised().ToString();
        }

        #endregion
    }
}