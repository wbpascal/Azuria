namespace Azuria.Enums
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
        Unkown = 1,

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
        ApiKeyNoPermission = 1004,

        /// <summary>
        /// </summary>
        LoginTokenInvalid = 1005,

        /// <summary>
        /// </summary>
        ApiFunctionBlocked = 1006,

        /// <summary>
        /// </summary>
        ProxerMaintenanceMode = 1007,

        /// <summary>
        /// </summary>
        ApiMaintenanceMode = 1008,

        /// <summary>
        /// </summary>
        IpBlocked = 2000,

        /// <summary>
        /// </summary>
        ServerError = 2001,

        /// <summary>
        /// </summary>
        LoginCredentialsMissing = 3000,

        /// <summary>
        /// </summary>
        LoginCredentialsWrong = 3001,

        /// <summary>
        /// </summary>
        NotificationsNotLoggedIn = 3002,

        /// <summary>
        /// </summary>
        UserinfoUserNotFound = 3003,

        /// <summary>
        /// </summary>
        UcpNotLoggedIn = 3004,

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
        InfoInvalidType = 3008,

        /// <summary>
        /// </summary>
        InfoNotLoggedIn = 3009,

        /// <summary>
        /// </summary>
        InfoAlreadyInList = 3010,

        /// <summary>
        /// </summary>
        InfoTooManyFavourites = 3011,

        /// <summary>
        /// </summary>
        LoginAlreadyLoggedIn = 3012,

        /// <summary>
        /// </summary>
        LoginDifferentUserLoggedIn = 3013,

        /// <summary>
        /// </summary>
        UserNoPermission = 3014,

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
        MangaChapterNotFound = 3019,

        /// <summary>
        /// </summary>
        AnimeNoStreamsAvailable = 3020,

        /// <summary>
        /// </summary>
        AnimeStreamNotFound = 3021,

        /// <summary>
        /// </summary>
        UcpEpisodeNotFound = 3022,

        /// <summary>
        /// </summary>
        MessengerNotLoggedIn = 3023,

        /// <summary>
        /// </summary>
        MessengerInvalidConference = 3024,

        /// <summary>
        /// </summary>
        MessengerReportTooShort = 3025,

        /// <summary>
        /// </summary>
        MessengerMessageInvalid = 3026,

        /// <summary>
        /// </summary>
        MessengerUserInvalid = 3027,

        /// <summary>
        /// </summary>
        MessengerUserLimitReached = 3028,

        /// <summary>
        /// </summary>
        MessengerTopicInvalid = 3029,

        /// <summary>
        /// </summary>
        MessengerNotEnoughUsers = 3030,

        /// <summary>
        /// </summary>
        ChatInvalidRoom = 3031,

        /// <summary>
        /// </summary>
        ChatNoPermission = 3032,

        /// <summary>
        /// </summary>
        ChatInvalidMessage = 3033,

        /// <summary>
        /// </summary>
        ChatNotLoggedIn = 3034,

        /// <summary>
        /// </summary>
        ListInvalidLanguage = 3035,

        /// <summary>
        /// </summary>
        ListInvalidType = 3036,

        /// <summary>
        /// </summary>
        ListInvalidId = 3037,

        /// <summary>
        /// </summary>
        User2FaKeyNeeded = 3038,

        /// <summary>
        /// </summary>
        UserAccountTimeout = 3039,

        /// <summary>
        /// </summary>
        UserAccountBlocked = 3040,

        /// <summary>
        /// </summary>
        UserInternalError = 3041,

        /// <summary>
        /// </summary>
        ErrorLogEmptyString = 3042
    }
}