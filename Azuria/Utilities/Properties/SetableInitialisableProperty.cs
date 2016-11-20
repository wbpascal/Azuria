using System;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Utilities.Properties
{
    /// <summary>
    /// Represents a property that can be initialised and set.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public class SetableInitialisableProperty<T> : InitialisableProperty<T>
    {
        private readonly Func<T, Task<IProxerResult>> _setMethod;

        internal SetableInitialisableProperty(Func<Task<IProxerResult>> initMethod,
            Func<T, Task<IProxerResult>> setMethod)
            : base(initMethod)
        {
            this._setMethod = setMethod;
            this.IsInitialised = false;
        }

        internal SetableInitialisableProperty(Func<Task<IProxerResult>> initMethod,
            Func<T, Task<IProxerResult>> setMethod,
            T initialisationResult) : base(initMethod, initialisationResult)
        {
            this._setMethod = setMethod;
            this.IsInitialised = true;
        }

        #region Methods

        /// <summary>
        /// Sets the value of the property to the value of <paramref name="newValue" />.
        /// </summary>
        /// <param name="newValue">The new value of the property.</param>
        /// <returns>If the action was successful and if it was, the current value of the property.</returns>
        public async Task<IProxerResult<T>> SetObject(T newValue)
        {
            IProxerResult lInvokeResult = await this._setMethod.Invoke(newValue);
            if (!lInvokeResult.Success) return new ProxerResult<T>(lInvokeResult.Exceptions);
            this.Set(newValue);
            return await this.Get();
        }

        #endregion
    }
}