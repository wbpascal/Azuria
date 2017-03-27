using Azuria.Enumerable;
using Azuria.Media.Properties;

namespace Azuria.Info.Enumerable
{
    /// <summary>
    /// 
    /// </summary>
    public class IndustryEnumerable : PagedEnumerable<Industry>
    {
        private readonly string _startWith;
        private readonly string _contains;
        private readonly Country? _country;
        private readonly IndustryType? _type;

        internal IndustryEnumerable(string startWith = "", string contains = "", Country? country = null,
            IndustryType? type = null)
        {
            this._startWith = startWith;
            this._contains = contains;
            this._country = country;
            this._type = type;
        }

        /// <inheritdoc />
        public override PagedEnumerator<Industry> GetEnumerator()
        {
            return new IndustryEnumerator(this._startWith, this._contains, this._country, this._type, 
                this.RetryCount);
        }
    }
}