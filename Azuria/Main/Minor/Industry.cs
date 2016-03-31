using JetBrains.Annotations;

namespace Azuria.Main.Minor
{
    /// <summary>
    ///     Eine Klasse, die die <see cref="Industry">Indurstrie</see> eines <see cref="Anime" /> oder <see cref="Manga" />
    ///     darstellt.
    /// </summary>
    public class Industry
    {
        /// <summary>
        ///     Eine Enumeration, die den Typen der <see cref="Industry">Industrie</see> eines <see cref="Anime" /> oder
        ///     <see cref="Manga" /> darstellt.
        /// </summary>
        public enum IndustryType
        {
            /// <summary>
            ///     Es handelt sich um einen Publisher.
            /// </summary>
            Publisher,

            /// <summary>
            ///     Es handelt sich um ein Studio.
            /// </summary>
            Studio,

            /// <summary>
            ///     Es handelt sich um einen Produzenten.
            /// </summary>
            Producer,

            /// <summary>
            ///     Es wurde nichts spezifiziert.
            /// </summary>
            None
        }

        internal Industry(int id, [NotNull] string name, IndustryType type)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
        }

        #region Properties

        /// <summary>
        ///     Gibt die ID der <see cref="Industry">Industrie</see> zurück.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gibt den Namen der <see cref="Industry">Industrie</see> zurück.
        /// </summary>
        [NotNull]
        public string Name { get; set; }

        /// <summary>
        ///     Gibt den <see cref="IndustryType">Typ</see> der <see cref="Industry">Industrie</see> zurück.
        /// </summary>
        public IndustryType Type { get; set; }

        #endregion
    }
}