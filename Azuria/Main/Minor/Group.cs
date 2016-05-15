using JetBrains.Annotations;

namespace Azuria.Main.Minor
{
    /// <summary>
    ///     Represents a class which describes a <see cref="Group"/>, who translates <see cref="Anime" /> or <see cref="Manga" />.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Represents an error.
        /// </summary>
        public static Group Error = new Group(-1, "ERROR");

        internal Group(int id, [NotNull] string name)
        {
            this.Id = id;
            this.Name = name;
        }

        #region Properties

        /// <summary>
        ///     Gets the id of the <see cref="Group"/>.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets the name of the <see cref="Group"/>.
        /// </summary>
        [NotNull]
        public string Name { get; }

        #endregion
    }
}