using System;
using System.Threading.Tasks;
using Azuria.Exceptions;
using JetBrains.Annotations;

namespace Azuria.Utilities.ErrorHandling
{
    internal class ErrorHandler
    {
        #region

        [ItemNotNull]
        internal static async Task<ProxerResult> HandleError(Senpai senpai, string wrongHtml, bool checkedLogin)
        {
            ProxerResult<bool> lCheckResult;
            if (!checkedLogin && (lCheckResult = await senpai.CheckLogin()).Success && !lCheckResult.Result)
            {
                return new ProxerResult(new Exception[] {new NotLoggedInException(senpai)});
            }

            return new ProxerResult(new Exception[] {new WrongResponseException {Response = wrongHtml}});
        }

        [NotNull]
        internal static ProxerResult HandleError(Senpai senpai, string wrongHtml)
        {
            return new ProxerResult(new Exception[] {new WrongResponseException {Response = wrongHtml}});
        }

        #endregion
    }
}