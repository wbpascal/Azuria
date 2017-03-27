using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azuria.Enumerable;
using Azuria.Media;
using Azuria.Media.Properties;

namespace Azuria.Info.Enumerable
{
    /// <summary>
    /// 
    /// </summary>
    public class TranslatorEnumerable : PagedEnumerable<Translator>
    {
        private readonly string _startsWith;
        private readonly string _contains;
        private readonly Country? _country;

        internal TranslatorEnumerable(string startsWith, string contains, Country? country)
        {
            this._startsWith = startsWith;
            this._contains = contains;
            this._country = country;
        }

        /// <inheritdoc />
        public override PagedEnumerator<Translator> GetEnumerator()
        {
            return new TranslatorEnumerator(this._startsWith, this._contains, this._country, this.RetryCount);
        }
    }
}
