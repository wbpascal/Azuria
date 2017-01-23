﻿using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Utilities.Properties
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IArgumentInitialisableProperty<in TIn, TOut>
    {
        #region Properties

        /// <summary>
        /// Gets a value whether the property was already initialised at least once.
        /// </summary>
        bool IsInitialised { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches a new value for the property without returning the new value.
        /// </summary>
        /// <param name="param"></param>
        /// <returns>If the action was successful.</returns>
        Task<IProxerResult> FetchObject(TIn param);

        /// <summary>
        /// Initialises the property if it is not already.
        /// </summary>
        /// <param name="param"></param>
        /// <returns>If the action was successful and if it was, the value of this property.</returns>
        Task<IProxerResult<TOut>> Get(TIn param);

        /// <summary>
        /// Initialises the property if it is not already.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="onError">A value that is returned if the action was not successful.</param>
        /// <returns>
        /// If the action was successful and if it was, the value of this property. If it was not then
        /// <paramref name="onError" /> is returned.
        /// </returns>
        Task<TOut> Get(TIn param, TOut onError);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        TOut GetIfInitialised();

        /// <summary>
        /// </summary>
        /// <param name="ifNot"></param>
        /// <returns></returns>
        TOut GetIfInitialised(TOut ifNot);

        /// <summary>
        /// Gets a new value for the property independent of it being already initialised.
        /// </summary>
        /// <param name="param"></param>
        /// <returns>If the action was successful and if it was, the value of this property.</returns>
        Task<IProxerResult<TOut>> GetNew(TIn param);

        /// <summary>
        /// Gets a new value for the property independent of it being already initialised.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="onError">A value that is returned if the action was not successful.</param>
        /// <returns>
        /// If the action was successful and if it was, the value of this property. If it was not then
        /// <paramref name="onError" /> is returned.
        /// </returns>
        Task<TOut> GetNew(TIn param, TOut onError);

        /// <summary>
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<TOut> ThrowFirstOnNonSuccess(TIn param);

        #endregion
    }
}