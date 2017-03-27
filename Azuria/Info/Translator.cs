using System;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Info.Enumerable;
using Azuria.Media;
using Azuria.Media.Properties;
using ListApi = Azuria.Api.v1.DataModels.List;

namespace Azuria.Info
{
    /// <summary>
    /// Represents a class which describes a person or group, who translates <see cref="Anime" /> or
    /// <see cref="Manga" />.
    /// </summary>
    public class Translator
    {
        internal Translator(int id, string name, Country country)
        {
            this.Id = id;
            this.Name = name;
            this.Country = country;
        }

        internal Translator(TranslatorDataModel dataModel)
            : this(dataModel.Id, dataModel.Name, dataModel.Country)
        {
            this.Image = dataModel.Image;
        }

        internal Translator(ListApi.TranslatorDataModel dataModel)
            : this(dataModel.Id, dataModel.Name, dataModel.Country)
        {
            this.Image = dataModel.Image;
        }

        #region Properties

        /// <summary>
        /// Gets the id of the <see cref="Translator" />.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the Language of the <see cref="Translator" />.
        /// </summary>
        public Country Country { get; internal set; }

        /// <summary>
        /// Gets the name of the <see cref="Translator" />.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Not always available.
        /// </summary>
        public Uri Image { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="includeH"></param>
        /// <returns></returns>
        public TranslatorProjectEnumerable GetProjects(TranslationStatus? status = null, bool? includeH = null)
        {
            return new TranslatorProjectEnumerable(this.Id, status, includeH);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startsWith"></param>
        /// <param name="contains"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public static TranslatorEnumerable GetList(string startsWith, string contains, Country? country)
        {
            return new TranslatorEnumerable(startsWith, contains, country);
        }
    }
}