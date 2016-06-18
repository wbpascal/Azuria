using System;
using JetBrains.Annotations;

namespace Azuria.AnimeManga
{
    /// <summary>
    ///     Represents a class that contains a <see cref="FskType" />-value and some additional informations associated with
    ///     the
    ///     value.
    /// </summary>
    public class FskObject
    {
        internal FskObject(FskType fskType, Uri fskPictureUri)
        {
            this.FskType = fskType;
            this.FskPictureUri = fskPictureUri;
        }

        #region Properties

        /// <summary>
        ///     Gets the <see cref="FskType" /> Info-Picture associated with this object.
        /// </summary>
        [NotNull]
        public Uri FskPictureUri { get; }

        /// <summary>
        ///     Gets the <see cref="FskType" />-value associated with this object.
        /// </summary>
        public FskType FskType { get; }

        #endregion
    }
}