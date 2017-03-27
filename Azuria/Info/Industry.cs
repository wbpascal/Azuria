using System.Threading.Tasks;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.List;
using Azuria.Info.Enumerable;
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="country"></param>
        public Industry(int id, string name, IndustryType type, Country country)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
            this.Country = country;
        }

        internal Industry(PublisherDataModel dataModel)
            : this(dataModel.Id, dataModel.Name, dataModel.Type, dataModel.Country)
        {
        }

        internal Industry(IndustryDataModel dataModel)
            : this(dataModel.Id, dataModel.Name, dataModel.Type, dataModel.Country)
        {
        }

        #region Properties

        /// <summary>
        /// Gets the country the <see cref="Industry" /> is from.
        /// </summary>
        public Country Country { get; }

        /// <summary>
        /// Gets the id of the <see cref="Industry" />.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the name of the <see cref="Industry" />.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the <see cref="IndustryType">type</see> of the <see cref="Industry" />.
        /// </summary>
        public IndustryType Type { get; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeH"></param>
        /// <returns></returns>
        public IndustryProjectsEnumerable GetProjects(IndustryType? type = null, bool? includeH = null)
        {
            return new IndustryProjectsEnumerable(this.Id, type, includeH);
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startWith"></param>
        /// <param name="contains"></param>
        /// <param name="country"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IndustryEnumerable GetList(string startWith = "", string contains = "",
            Country? country = null, IndustryType? type = null)
        {
            return new IndustryEnumerable(startWith, contains, country, type);
        }

        #endregion
    }
}