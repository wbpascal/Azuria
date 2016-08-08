using Azuria.Api.v1.DataModels.Info;

namespace Azuria.AnimeManga.Properties
{
    /// <summary>
    /// </summary>
    public class Tag
    {
        internal Tag(EntryTagDataModel dataModel)
        {
            this.TagType = dataModel.Tag;
            this.IsRated = dataModel.IsRated;
            this.IsSpoiler = dataModel.IsSpoiler;
        }

        #region Properties

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