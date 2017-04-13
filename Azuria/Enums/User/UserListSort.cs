using System.ComponentModel;

namespace Azuria.Enums.User
{
    /// <summary>
    /// 
    /// </summary>
    public enum UserListSort
    {
        /// <summary>
        /// Order by entry name.
        /// </summary>
        [Description("nameASC")] Name,

        /// <summary>
        /// Order first by status of the entry and then by entry name.
        /// </summary>
        [Description("stateNameASC")] StateName,

        /// <summary>
        /// Order by last changed ascending.
        /// </summary>
        [Description("changeDateASC")] ChangeDate,

        /// <summary>
        /// Order first by status of the entry and then by last changed.
        /// </summary>
        [Description("stateChangeDateASC")] StateChangeDate
    }
}