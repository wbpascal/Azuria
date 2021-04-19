using Azuria.ErrorHandling;
using Azuria.Requests.Builder;

namespace Azuria.Authentication
{
    /// <summary>
    /// </summary>
    public interface ILoginManager
    {
        /// <summary>
        /// Adds the authentication information to the request if needed.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>If any information was added to the request</returns>
        bool AddAuthenticationInformation(IRequestBuilderBase request);

        /// <summary>
        /// Checks if the request contains all needed information for authentication.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool ContainsAuthenticationInformation(IRequestBuilderBase request);

        /// <summary>
        /// Invalidates the current login session so that the authentication information will be added the next time
        /// <see cref="AddAuthenticationInformation"/> is called.
        /// </summary>
        void InvalidateLogin();

        /// <summary>
        /// Assumes that the client was just logged in with the given login token. Used to manually set the login token
        /// when the automatic interception cannot be used.
        /// </summary>
        /// <param name="loginToken"></param>
        void SetLogin(char[] loginToken);

        /// <summary>
        /// Updates the state of the login manager with the information from a request and the corresponding result.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="result"></param>
        void Update(IRequestBuilderBase request, IProxerResultBase result);
    }
}