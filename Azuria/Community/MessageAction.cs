namespace Azuria.Community
{
    /// <summary>
    /// The action of the message.
    /// </summary>
    public enum MessageAction
    {
        /// <summary>
        /// Normal message, only text content.
        /// </summary>
        NoAction,

        /// <summary>
        /// A user was added to the conference.
        /// </summary>
        AddUser,

        /// <summary>
        /// A user was removed from the conference.
        /// </summary>
        RemoveUser,

        /// <summary>
        /// The leader of the conference was changed.
        /// </summary>
        SetLeader,

        /// <summary>
        /// The topic of the conference was changed.
        /// </summary>
        SetTopic
    }
}