using Azuria.Api.v1.Input.Converter;
using Azuria.Enums;
using Azuria.Enums.User;
using Azuria.Helpers.Attributes;
using Azuria.Helpers.Extensions;

namespace Azuria.Api.v1.Input.Ucp
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class UcpGetListInputBase : PagedInputDataModel
    {
        /// <summary>
        /// Optional. The category that should be loaded.
        /// </summary>
        [InputData("kat", Converter = typeof(ToLowerConverter))]
        public MediaEntryType Category { get; set; } = MediaEntryType.Anime;

        /// <summary>
        /// Optional. The string that all returned entries should contain.
        /// </summary>
        [InputData("search", Optional = true)]
        public string Search { get; set; }

        /// <summary>
        /// Optional. The string that all returned entries should start with.
        /// </summary>
        [InputData("search_start", Optional = true)]
        public string SearchStart { get; set; }

        /// <summary>
        /// Optional. The order in which the returned entries should be returned.
        /// </summary>
        [InputData("sort", ConverterMethodName = nameof(GetSortString))]
        public UserListSort Sort { get; set; } = UserListSort.StateName;

        /// <summary>
        /// </summary>
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        internal string GetSortString(UserListSort sort)
        {
            return sort.GetDescription() + this.SortDirection.GetDescription();
        }
    }
}