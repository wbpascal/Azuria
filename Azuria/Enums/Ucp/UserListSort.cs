using System.ComponentModel;

namespace Azuria.Enums.Ucp
{
    /// <summary>
    /// 
    /// </summary>
    public enum UserListSort
    {
        /// <summary>
        /// Order by entry name ascending.
        /// </summary>
        [Description("nameASC")] NameAsc,

        /// <summary>
        /// Order by entry name descending.
        /// </summary>
        [Description("nameDESC")] NameDesc,

        /// <summary>
        /// Order first by status of the entry and then by entry name ascending.
        /// </summary>
        [Description("stateNameASC")] StateNameAsc,

        /// <summary>
        /// Order first by status of the entry and then by entry name descending.
        /// </summary>
        [Description("stateNameDESC")] StateNameDesc,

        /// <summary>
        /// Order by last changed ascending.
        /// </summary>
        [Description("changeDateASC")] ChangeDateAsc,

        /// <summary>
        /// Order by last changed descending.
        /// </summary>
        [Description("changeDateDESC")] ChangeDateDesc,

        /// <summary>
        /// Order first by status of the entry and then by last changed ascending.
        /// </summary>
        [Description("stateChangeDateASC")] StateChangeDateAsc,

        /// <summary>
        /// Order first by status of the entry and then by last changed descending.
        /// </summary>
        [Description("stateChangeDateDESC")] StateChangeDateDesc
    }
}