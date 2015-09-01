namespace Proxer.API.Main.Minor
{
    /// <summary>
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
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// </summary>
        public string Name { get; private set; }

        #endregion
    }
}