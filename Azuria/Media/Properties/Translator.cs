using Azuria.Api.v1.DataModels.Info;

namespace Azuria.Media.Properties
{
    /// <summary>
    ///     Represents a class which describes a <see cref="Translator" />, who translates <see cref="Anime" /> or
    ///     <see cref="Manga" />.
    /// </summary>
    public class Translator
    {
        internal Translator(int id, string name, Language language)
        {
            this.Id = id;
            this.Name = name;
            this.Language = language;
        }

        internal Translator(TranslatorDataModel dataModel) : this(dataModel.Id, dataModel.Name, dataModel.Language)
        {
        }

        #region Properties

        /// <summary>
        ///     Gets the id of the <see cref="Translator" />.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets the Language of the <see cref="Translator" />.
        /// </summary>
        public Language Language { get; internal set; }

        /// <summary>
        ///     Gets the name of the <see cref="Translator" />.
        /// </summary>
        public string Name { get; }

        #endregion
    }
}