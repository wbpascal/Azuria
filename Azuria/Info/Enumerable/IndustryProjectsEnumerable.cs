using Azuria.Enumerable;
using Azuria.Media;

namespace Azuria.Info.Enumerable
{
    /// <summary>
    /// 
    /// </summary>
    public class IndustryProjectsEnumerable : PagedEnumerable<IMediaObject>
    {
        private readonly int _id;
        private readonly IndustryType? _type;
        private readonly bool? _includeH;

        internal IndustryProjectsEnumerable(int id, IndustryType? type, bool? includeH)
        {
            this._id = id;
            this._type = type;
            this._includeH = includeH;
        }

        /// <inheritdoc />
        public override PagedEnumerator<IMediaObject> GetEnumerator()
        {
            return new IndustryProjectsEnumerator(this._id, this._type, this._includeH);
        }
    }
}
