using Azuria.Api.v1.DataModels.Info;

namespace Azuria.Media.Properties
{
    /// <summary>
    /// </summary>
    public class Tag
    {
        internal Tag(EntryTagDataModel dataModel)
        {
            this.Description = dataModel.Description;
            this.IsRated = dataModel.IsRated;
            this.IsSpoiler = dataModel.IsSpoiler;
            this.TagType = dataModel.Tag;
        }

        #region Properties

        /// <summary>
        /// Only available in german.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        public bool IsRated { get; set; }

        /// <summary>
        /// </summary>
        public bool IsSpoiler { get; set; }

        /// <summary>
        /// </summary>
        public TagType TagType { get; set; }

        #endregion
    }
}