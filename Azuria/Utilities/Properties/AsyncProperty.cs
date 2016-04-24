using System;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Utilities.Properties
{
    /// <summary>
    /// 
    /// </summary>
    public class AsyncProperty<T>
    {
        private T _currentValue;
        [CanBeNull] private readonly Func<Task<ProxerResult<T>>> _getFunc;
        [CanBeNull] private readonly Func<T, Task<ProxerResult>> _setFunc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialValue"></param>
        /// <param name="getFunc"></param>
        /// <param name="setFunc"></param>
        public AsyncProperty(T initialValue, Func<Task<ProxerResult<T>>> getFunc = null, Func<T, Task<ProxerResult>> setFunc = null)
        {
            this._currentValue = initialValue;
            this._getFunc = getFunc;
            this._setFunc = setFunc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ProxerResult<T>> Get()
        {
            if (this._getFunc == null) return new ProxerResult<T>(this._currentValue);

            ProxerResult<T> lGetObjectResult = await this._getFunc.Invoke();
            if (!lGetObjectResult.Success || lGetObjectResult.Result == null)
                return new ProxerResult<T>(lGetObjectResult.Exceptions);

            this._currentValue = lGetObjectResult.Result;
            return new ProxerResult<T>(this._currentValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public async Task<ProxerResult> Set(T newValue)
        {
            if (this._setFunc == null) this._currentValue = newValue;
            else
            {
                ProxerResult lSetObjectResult = await this._setFunc.Invoke(newValue);
                if(!lSetObjectResult.Success)
                    return new ProxerResult(lSetObjectResult.Exceptions);

                this._currentValue = newValue;
            }

            return new ProxerResult();
        }
    }
}
