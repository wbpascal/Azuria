using System;

namespace Azuria.Main.Minor
{
    /// <summary>
    /// 
    /// </summary>
    public class Genre
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Uri WikiLink => new Uri("https://proxer.me/wiki/" + this.Name);

        internal Genre(string name)
        {
            this.Name = name;
        }
    }
}
