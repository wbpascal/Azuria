using System;
using Azuria.Exceptions;

namespace Azuria.Utilities.ErrorHandling
{
    internal class ErrorHandler
    {
        #region Methods

        internal static ProxerResult HandleError(string wrongHtml, bool checkedLogin)
        {
            return new ProxerResult(new Exception[] {new WrongResponseException {Response = wrongHtml}});
        }

        internal static ProxerResult HandleError(string wrongHtml)
        {
            return new ProxerResult(new Exception[] {new WrongResponseException {Response = wrongHtml}});
        }

        #endregion
    }
}