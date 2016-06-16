using System;
using JetBrains.Annotations;

namespace Azuria.AnimeManga
{
    /// <summary>
    ///     Represents a class that contains a <see cref="Fsk" />-value and some additional informations associated with the
    ///     value.
    /// </summary>
    public class FskObject
    {
        internal FskObject(Fsk fsk, Uri fskPictureUri)
        {
            this.Fsk = fsk;
            this.FskPictureUri = fskPictureUri;
        }

        #region Properties

        /// <summary>
        ///     Gets the <see cref="Fsk" />-value associated with this object.
        /// </summary>
        public Fsk Fsk { get; }

        /// <summary>
        ///     Gets the <see cref="Fsk" /> Info-Picture associated with this object.
        /// </summary>
        [NotNull]
        public Uri FskPictureUri { get; }

        #endregion
    }
}