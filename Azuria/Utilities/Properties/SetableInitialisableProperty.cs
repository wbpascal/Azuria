using System;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Utilities.Properties
{
    /// <summary>
    ///     Represents a property that can be initialised and set.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public class SetableInitialisableProperty<T> : InitialisableProperty<T>
    {
        private readonly Func<T, Task<ProxerResult>> _setMethod;

        internal SetableInitialisableProperty(Func<Task<ProxerResult>> initMethod, Func<T, Task<ProxerResult>> setMethod)
            : base(initMethod)
        {
            this._setMethod = setMethod;
            this.IsInitialisedOnce = false;
        }

        internal SetableInitialisableProperty(Func<Task<ProxerResult>> initMethod, Func<T, Task<ProxerResult>> setMethod,
            T initialisationResult) : base(initMethod, initialisationResult)
        {
            this._setMethod = setMethod;
            this.IsInitialisedOnce = true;
        }

        #region Methods

        /// <summary>
        ///     Sets the value of the property to the value of <paramref name="newValue" />.
        /// </summary>
        /// <param name="newValue">The new value of the property.</param>
        /// <returns>If the action was successful and if it was, the current value of the property.</returns>
        public async Task<ProxerResult<T>> SetObject(T newValue)
        {
            ProxerResult lInvokeResult = await this._setMethod.Invoke(newValue);
            if (!lInvokeResult.Success) return new ProxerResult<T>(lInvokeResult.Exceptions);
            this.SetInitialisedObject(newValue);
            return await this.GetObject();
        }

        #endregion
    }
}