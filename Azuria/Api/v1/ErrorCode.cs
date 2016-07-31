namespace Azuria.Api.v1
{
    /// <summary>
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// </summary>
        NoError = 0,

        /// <summary>
        /// </summary>
        ApiVersionNotFound = 1000,

        /// <summary>
        /// </summary>
        ApiVersionDeleted = 1001,

        /// <summary>
        /// </summary>
        ApiClassNotFound = 1002,

        /// <summary>
        /// </summary>
        ApiFunctionNotFound = 1003,

        /// <summary>
        /// </summary>
        ApiKeyInsufficientPermissions = 1004,

        /// <summary>
        /// </summary>
        LoginTokenInvalid = 1005,

        /// <summary>
        /// </summary>
        ApiFunctionBlocked = 1006,

        /// <summary>
        /// </summary>
        IpBlocked = 2000,

        /// <summary>
        /// </summary>
        NewsRequestError = 2001,

        /// <summary>
        /// </summary>
        LoginCredentialsMissing = 3000,

        /// <summary>
        /// </summary>
        LoginCredentialsWrong = 3001,

        /// <summary>
        /// </summary>
        NotificationsUserNotLoggedIn = 3002,

        /// <summary>
        /// </summary>
        UserinfoUserIdNotFound = 3003,

        /// <summary>
        /// </summary>
        UcpUserNotLoggedIn = 3004,

        /// <summary>
        /// </summary>
        UcpCategoryNotFound = 3005,

        /// <summary>
        /// </summary>
        UcpInvalidId = 3006,

        /// <summary>
        /// </summary>
        InfoInvalidId = 3007,

        /// <summary>
        /// </summary>
        InfoSetUserInfoInvalidType = 3008,

        /// <summary>
        /// </summary>
        InfoSetUserInfoUserNotLoggedIn = 3009,

        /// <summary>
        /// </summary>
        InfoSetUserInfoAlreadyPresentInList = 3010,

        /// <summary>
        /// </summary>
        InfoSetUserInfoTooManyFavourites = 3011,

        /// <summary>
        /// </summary>
        LoginUserAlreadyLoggedIn = 3012,

        /// <summary>
        /// </summary>
        LoginDifferentUserLoggedIn = 3013,

        /// <summary>
        /// </summary>
        UserInsufficientPermissions = 3014,

        /// <summary>
        /// </summary>
        ListCategoryNotFound = 3015,

        /// <summary>
        /// </summary>
        ListMediumNotFound = 3016,

        /// <summary>
        /// </summary>
        MediaStyleNotFound = 3017,

        /// <summary>
        /// </summary>
        MediaEntryNotFound = 3018,

        /// <summary>
        /// </summary>
        MediaChapterNotFound = 3019,

        /// <summary>
        /// </summary>
        MediaNoStreamsAvailable = 3020,

        /// <summary>
        /// </summary>
        MediaStreamNotFound = 3021
    }
}