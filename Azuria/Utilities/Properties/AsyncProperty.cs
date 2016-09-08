using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Utilities.Properties
{
    /// <summary>
    ///     Represents a property where the get- and set-method is asynchronous.
    /// </summary>
    public class AsyncProperty<T>
    {
        private readonly Func<Task<ProxerResult<T>>> _getFunc;
        private readonly Func<T, Task<ProxerResult>> _setFunc;
        private T _currentValue;

        /// <summary>
        ///     Initialises a new instance with an initial value and optional get- and set-methods.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        /// <param name="getFunc">
        ///     An optional get-method. If not set the <see cref="Get" />-method always just returns the current
        ///     value.
        /// </param>
        /// <param name="setFunc">
        ///     An optional set-method. If not set the <see cref="Set" />-method always just sets the value to
        ///     the passed one.
        /// </param>
        public AsyncProperty(T initialValue, Func<Task<ProxerResult<T>>> getFunc = null,
            Func<T, Task<ProxerResult>> setFunc = null)
        {
            this._currentValue = initialValue;
            this._getFunc = getFunc;
            this._setFunc = setFunc;
        }

        #region Methods

        /// <summary>
        ///     Executes, if specified, the get-function that was specified in the constructor and then returns the current value.
        ///     If the function was not set just the current value will be returned.
        /// </summary>
        /// <returns>If the action was successful and if it was, the new value.</returns>
        public async Task<ProxerResult<T>> Get()
        {
            if (this._getFunc == null) return new ProxerResult<T>(this._currentValue);

            ProxerResult<T> lGetObjectResult = await this._getFunc.Invoke();
            if (!lGetObjectResult.Success || (lGetObjectResult.Result == null))
                return new ProxerResult<T>(lGetObjectResult.Exceptions);

            this._currentValue = lGetObjectResult.Result;
            return new ProxerResult<T>(this._currentValue);
        }

        private async Task<T> GetAndThrow()
        {
            ProxerResult<T> lResult = await this.Get();
            if (!lResult.Success || (lResult.Result == null))
                throw lResult.Exceptions.FirstOrDefault() ?? new Exception();

            return lResult.Result;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public TaskAwaiter<T> GetAwaiter()
        {
            return this.GetAndThrow().GetAwaiter();
        }

        /// <summary>
        ///     Executes, if specified, the set-funtion that was specified in the cunstructor and then sets the current value to
        ///     the value of <paramref name="newValue" />. If the function was not set the current value will only be set to the
        ///     value of <paramref name="newValue" />.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns>If the action was successful.</returns>
        public async Task<ProxerResult> Set(T newValue)
        {
            if (this._setFunc == null) this._currentValue = newValue;
            else
            {
                ProxerResult lSetObjectResult = await this._setFunc.Invoke(newValue);
                if (!lSetObjectResult.Success)
                    return new ProxerResult(lSetObjectResult.Exceptions);

                this._currentValue = newValue;
            }

            return new ProxerResult();
        }

        #endregion
    }
}