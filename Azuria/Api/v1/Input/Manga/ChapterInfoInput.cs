using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Info;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Manga
{
    /// <summary>
    /// 
    /// </summary>
    public class ChapterInfoInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the chapter number.
        /// </summary>
        [InputData("episode")]
        public int Chapter { get; set; }

        /// <summary>
        /// Gets or sets the id of the manga.
        /// </summary>
        [InputData("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the language of the returned chapter.
        /// 
        /// Invalid Values:
        /// * <see cref="Azuria.Enums.Info.Language.Unkown"/>
        /// </summary>
        [InputData(
            "language", Converter = typeof(ToShortStringConverter), ForbiddenValues = new object[] {Language.Unkown})]
        public Language Language { get; set; }
    }
}