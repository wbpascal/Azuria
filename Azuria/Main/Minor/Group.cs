namespace Azuria.Main.Minor
{
    /// <summary>
    ///     Eine Klasse, die eine <see cref="Group">Gruppe</see> darstellt, die <see cref="Anime" /> oder <see cref="Manga" />
    ///     übersetzt.
    /// </summary>
    public class Group
    {
        internal Group(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        #region Properties

        /// <summary>
        ///     Gibt die ID der <see cref="Group">Gruppe</see> zurück.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        ///     Gibt den Namen der <see cref="Group">Gruppe</see> zurück.
        /// </summary>
        public string Name { get; private set; }

        #endregion
    }
}