using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Azuria.Helpers.Attributes;
using Azuria.Helpers.Extensions;

namespace Azuria.Api.v1.Input.List
{
    /// <summary>
    /// 
    /// </summary>
    public class TagIdSearchInput : InputDataModel
    {
        /// <summary>
        /// </summary>
        [InputData("search", ConverterMethodName = nameof(GetSearchString))]
        public IEnumerable<string> TagsInclude { get; set; }

        /// <summary>
        /// </summary>
        public IEnumerable<string> TagsExclude { get; set; }

        private string GetSearchString(IEnumerable tagsInclude)
        {
            return $"{tagsInclude?.ToString(" ") ?? string.Empty} -{this.TagsExclude?.ToString(" -") ?? string.Empty}"
                .Trim();
        }
    }
}