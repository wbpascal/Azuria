namespace Proxer.API.Notifications
{
    /// <summary>
    ///     Eine Enumeration, die den Typen der Benachrichtigung darstellt.
    /// </summary>
    public enum NotificationObjectType
    {
        /// <summary>
        ///     Die Benachrichtigung ist eine <see cref="FriendRequestObject">Freundschaftsanfrage</see>.
        /// </summary>
        FriendRequest,

        /// <summary>
        ///     Die Benachrichtigung ist eine <see cref="NewsObject">Nachricht</see>.
        /// </summary>
        News,

        /// <summary>
        ///     Die Benachrichtigung ist eine <see cref="PmObject">Private Nachricht</see>.
        /// </summary>
        PrivateMessage,

        /// <summary>
        ///     Die Benachrichtigung ist eine neue <see cref="Main.Anime.Episode">Episode</see> oder ein neues
        ///     <see cref="Main.Manga.Chapter">Kapitel</see>.
        /// </summary>
        AnimeManga
    };
}