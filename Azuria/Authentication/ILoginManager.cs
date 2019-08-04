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
        /// Checks if the client is probably logged in at the moment(results may be inaccurate).
        /// </summary>
        /// <returns>A boolean that indicates if the client is probably logged in.</returns>
        bool IsLoginProbablyValid();

        /// <summary>
        /// Updates the state of the login manager with the information from the result
        /// </summary>
        /// <param name="result"></param>
        /// <param name="includedAuthInfo">Whether the request included the authentication information</param>
        void Update(IProxerResultBase result, bool includedAuthInfo = false);
    }
}