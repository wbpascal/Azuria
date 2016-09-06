using Azuria.Api.v1.DataModels.Info;

namespace Azuria.AnimeManga.Properties
{
    /// <summary>
    ///     Represents a class which describes the <see cref="Industry" /> of an <see cref="Anime" /> or <see cref="Manga" />.
    /// </summary>
    public class Industry
    {
        /// <summary>
        ///     Represents an enumeration which describes the type of the <see cref="Industry" />.
        /// </summary>
        public enum IndustryType
        {
            /// <summary>
            ///     Represents a publisher.
            /// </summary>
            Publisher,

            /// <summary>
            ///     Represents a studio.
            /// </summary>
            Studio,

            /// <summary>
            ///     Represents a producer.
            /// </summary>
            Producer,

            /// <summary>
            ///     Represents an unkown type.
            /// </summary>
            Unknown
        }

        /// <summary>
        ///     Represents an error.
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
        ///     Gets the country the <see cref="Industry" /> is from.
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        ///     Gets the id of the <see cref="Industry" />.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets the name of the <see cref="Industry" />.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets the <see cref="IndustryType">type</see> of the <see cref="Industry" />.
        /// </summary>
        public IndustryType Type { get; set; }

        #endregion
    }
}