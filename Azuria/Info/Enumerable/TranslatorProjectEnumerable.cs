using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azuria.Enumerable;

namespace Azuria.Info.Enumerable
{
    /// <summary>
    /// 
    /// </summary>
    public class TranslatorProjectEnumerable : PagedEnumerable<TranslatorProject>
    {
        private readonly int _id;
        private readonly TranslationStatus? _status;
        private readonly bool? _includeH;

        internal TranslatorProjectEnumerable(int id, TranslationStatus? status = null, bool? includeH = null)
        {
            this._id = id;
            this._status = status;
            this._includeH = includeH;
        }

        /// <inheritdoc />
        public override PagedEnumerator<TranslatorProject> GetEnumerator()
        {
            return new TranslatorProjectEnumerator(this._id, this._status, this._includeH, this.RetryCount);
        }
    }
}
