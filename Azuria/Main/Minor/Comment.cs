using System.Collections.Generic;

namespace Azuria.Main.Minor
{
    /// <summary>
    /// </summary>
    public class Comment
    {
        internal Comment(Azuria.User autor, int sterne, string kommentar)
        {
            this.Autor = autor;
            this.Sterne = sterne;
            this.Kommentar = kommentar;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public Azuria.User Autor { get; set; }

        /// <summary>
        /// </summary>
        public string Kommentar { get; set; }

        /// <summary>
        /// </summary>
        public int Sterne { get; set; }

        /// <summary>
        /// </summary>
        public Dictionary<string, int> SubSterne { get; set; }

        #endregion
    }
}