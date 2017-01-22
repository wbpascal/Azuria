using Azuria.Api.v1.DataModels.Info;
using Azuria.Media;
using Azuria.Media.Properties;

namespace Azuria.Info
{
    /// <summary>
    /// Represents a class which describes the <see cref="Industry" /> of an <see cref="Anime" /> or <see cref="Manga" />.
    /// </summary>
    public class Industry
    {
        /// <summary>
        /// Represents an error.
        /// </summary>
        public static Industry Error = new Industry(new PublisherDataModel {Id = -1, Name = "ERROR"});

        internal Industry(PublisherDataModel dataModel)
        {
            this.Id = dataModel.Id;
            this.Name = dataModel.Name;
            this.Type = dataModel.Type;
            this.Country = dataModel.Country;
        }

        #region Properties

        /// <summary>
        /// Gets the country the <see cref="Industry" /> is from.
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        /// Gets the id of the <see cref="Industry" />.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets the name of the <see cref="Industry" />.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the <see cref="IndustryType">type</see> of the <see cref="Industry" />.
        /// </summary>
        public IndustryType Type { get; set; }

        #endregion
    }
}