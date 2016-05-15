using System;
using JetBrains.Annotations;

namespace Azuria.Main.Minor
{
    /// <summary>
    /// Represents a class that contains a <see cref="Fsk"/>-value and some additional informations associated with the value.
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
        /// </summary>
        public Fsk Fsk { get; }

        /// <summary>
        /// </summary>
        [NotNull]
        public Uri FskPictureUri { get; }

        #endregion
    }
}