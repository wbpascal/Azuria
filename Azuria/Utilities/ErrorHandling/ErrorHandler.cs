using System;
using Azuria.Exceptions;
using JetBrains.Annotations;

namespace Azuria.Utilities.ErrorHandling
{
    internal class ErrorHandler
    {
        #region

        internal static ProxerResult HandleError(string wrongHtml, bool checkedLogin)
        {
            return new ProxerResult(new Exception[] {new WrongResponseException {Response = wrongHtml}});
        }

        [NotNull]
        internal static ProxerResult HandleError(string wrongHtml)
        {
            return new ProxerResult(new Exception[] {new WrongResponseException {Response = wrongHtml}});
        }

        #endregion
    }
}