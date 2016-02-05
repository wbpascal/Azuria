using System;
using System.Collections.Generic;

namespace Azuria.Main.Minor
{
    /// <summary>
    /// 
    /// </summary>
    public class GenreObject
    {
        /// <summary>
        /// 
        /// </summary>
        public enum GenreType
        {
            /// <summary>
            /// 
            /// </summary>
            Abenteuer,
            /// <summary>
            /// 
            /// </summary>
            Action,
            /// <summary>
            /// 
            /// </summary>
            Adult,
            /// <summary>
            /// 
            /// </summary>
            Comedy,
            /// <summary>
            /// 
            /// </summary>
            Cyberpunk,
            /// <summary>
            /// 
            /// </summary>
            Drama,
            /// <summary>
            /// 
            /// </summary>
            Ecchi,
            /// <summary>
            /// 
            /// </summary>
            Fantasy,
            /// <summary>
            /// 
            /// </summary>
            Harem,
            /// <summary>
            /// 
            /// </summary>
            Historical,
            /// <summary>
            /// 
            /// </summary>
            Horror,
            /// <summary>
            /// 
            /// </summary>
            Josei,
            /// <summary>
            /// 
            /// </summary>
            Magic,
            /// <summary>
            /// 
            /// </summary>
            MartialArt,
            /// <summary>
            /// 
            /// </summary>
            Mecha,
            /// <summary>
            /// 
            /// </summary>
            Military,
            /// <summary>
            /// 
            /// </summary>
            Musik,
            /// <summary>
            /// 
            /// </summary>
            Mystery,
            /// <summary>
            /// 
            /// </summary>
            None,
            /// <summary>
            /// 
            /// </summary>
            Psychological,
            /// <summary>
            /// 
            /// </summary>
            Romance,
            /// <summary>
            /// 
            /// </summary>
            School,
            /// <summary>
            /// 
            /// </summary>
            SciFi,
            /// <summary>
            /// 
            /// </summary>
            Seinen,
            /// <summary>
            /// 
            /// </summary>
            Shoujou,
            /// <summary>
            /// 
            /// </summary>
            ShoujouAi,
            /// <summary>
            /// 
            /// </summary>
            Shounen,
            /// <summary>
            /// 
            /// </summary>
            ShounenAi,
            /// <summary>
            /// 
            /// </summary>
            SliceOfLife,
            /// <summary>
            /// 
            /// </summary>
            Splatter,
            /// <summary>
            /// 
            /// </summary>
            Sport,
            /// <summary>
            /// 
            /// </summary>
            Superpower,
            /// <summary>
            /// 
            /// </summary>
            Vampire,
            /// <summary>
            /// 
            /// </summary>
            Violence,
            /// <summary>
            /// 
            /// </summary>
            Yaoi,
            /// <summary>
            /// 
            /// </summary>
            Yuri
        }

        internal static Dictionary<string, GenreType> TypeDictionary => new Dictionary<string,GenreType>
        {
            {"Abenteuer", GenreType.Abenteuer },
            {"Action", GenreType.Action },
            {"Adult", GenreType.Adult },
            {"Comedy", GenreType.Comedy },
            {"Cyberpunk", GenreType.Cyberpunk },
            {"Drama", GenreType.Drama },
            {"Ecchi", GenreType.Ecchi },
            {"Fantasy", GenreType.Fantasy },
            {"Harem", GenreType.Harem },
            {"Historical", GenreType.Historical },
            {"Horror", GenreType.Horror },
            {"Josei", GenreType.Josei },
            {"Magic", GenreType.Magic },
            {"Martial-Art", GenreType.MartialArt },
            {"Mecha", GenreType.Mecha },
            {"Military", GenreType.Military },
            {"Musik", GenreType.Musik },
            {"Mystery", GenreType.Mystery },
            {"Psychological", GenreType.Psychological },
            {"Romance", GenreType.Romance },
            {"School", GenreType.School },
            {"SciFi", GenreType.SciFi },
            {"Seinen", GenreType.Seinen },
            {"Shoujou", GenreType.Shoujou },
            {"Shoujou-Ai", GenreType.ShoujouAi },
            {"Shounen", GenreType.Shounen },
            {"Shounen-Ai", GenreType.ShounenAi },
            {"Slice_Of_Life", GenreType.SliceOfLife },
            {"Splatter", GenreType.Splatter },
            {"Sport", GenreType.Sport },
            {"Superpower", GenreType.Superpower },
            {"Vampire", GenreType.Vampire },
            {"Violence", GenreType.Violence },
            {"Yaoi", GenreType.Yaoi },
            {"Yuri", GenreType.Yuri }
        }; 

        /// <summary>
        /// 
        /// </summary>
        public GenreType Genre { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Uri WikiLink => new Uri("https://proxer.me/wiki/" + this.Genre);

        internal GenreObject(string name)
        {
            this.Genre = TypeDictionary.ContainsKey(name) ? TypeDictionary[name] : GenreType.None;
        }
    }
}
