using Azuria.Api.v1.Input.Converter;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Ucp
{
    /// <summary>
    /// 
    /// </summary>
    public class SetReminderInput : InputDataModel
    {
        /// <summary>
        /// TODO: Complete this
        /// "A value indicating whether the reminder is from an anime or manga (I don't know why we need
        /// this)."
        /// </summary>
        [InputData("kat", Converter = typeof(ToLowerConverter))]
        public MediaEntryType Category { get; set; }

        /// <summary>
        /// Gets or sets the id of the entry of which a reminder will be created.
        /// </summary>
        [InputData("id")]
        public int EntryId { get; set; }

        /// <summary>
        /// Gets or sets the number of the episode/chapter the reminder will be created of.
        /// </summary>
        [InputData("episode")]
        public int Episode { get; set; }

        /// <summary>
        /// TODO: Complete this
        /// Gets or sets the language of the episode/chapter 
        /// </summary>
        [InputData(
            "language", Converter = typeof(ToTypeStringConverter),
            ForbiddenValues = new object[] {MediaLanguage.Unkown})]
        public MediaLanguage Language { get; set; }
    }
}