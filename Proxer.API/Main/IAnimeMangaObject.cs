using System;
using System.Collections.Generic;
using Proxer.API.Main.Minor;

namespace Proxer.API.Main
{
    /// <summary>
    /// </summary>
    public interface IAnimeMangaObject
    {
        /// <summary>
        /// </summary>
        string Beschreibung { get; }

        /// <summary>
        /// </summary>
        string EnglischTitel { get; }

        /// <summary>
        /// </summary>
        Uri[] Fsk { get; }

        /// <summary>
        /// </summary>
        string[] Genre { get; }

        /// <summary>
        /// </summary>
        Group Gruppen { get; }

        /// <summary>
        /// </summary>
        int Id { get; }

        /// <summary>
        /// </summary>
        Industry[] Industrie { get; }

        /// <summary>
        /// </summary>
        string JapanTitel { get; }

        /// <summary>
        /// </summary>
        bool Lizensiert { get; }

        /// <summary>
        /// </summary>
        string Name { get; }

        /// <summary>
        /// </summary>
        AnimeMangaType ObjectType { get; }

        /// <summary>
        ///     Jahr | Winter/Frühling/Sommer/Herbst
        /// </summary>
        Dictionary<int, int> Season { get; }

        /// <summary>
        /// </summary>
        string Synonym { get; }
    }

    /// <summary>
    /// </summary>
    public enum AnimeMangaType
    {
        /// <summary>
        /// </summary>
        Anime,

        /// <summary>
        /// </summary>
        Manga
    }

    /// <summary>
    /// </summary>
    public enum AnimeMangaStatus
    {
        /// <summary>
        /// </summary>
        PreAiring,

        /// <summary>
        /// </summary>
        Airing,

        /// <summary>
        /// </summary>
        Abgebrochen,

        /// <summary>
        /// </summary>
        Abgeschlossen
    }
}