namespace Proxer.API.Main.Minor
{
    /// <summary>
    /// </summary>
    public class Industry
    {
        /// <summary>
        /// </summary>
        public enum IndustryType
        {
            /// <summary>
            /// </summary>
            Publisher,

            /// <summary>
            /// </summary>
            Studio,

            /// <summary>
            /// </summary>
            Producer,

            /// <summary>
            /// </summary>
            None
        }

        internal Industry(int id, string name, IndustryType type)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        public IndustryType Type { get; set; }

        #endregion
    }
}