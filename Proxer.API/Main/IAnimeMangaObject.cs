using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proxer.API.Main.Minor;

namespace Proxer.API.Main
{
    /// <summary>
    /// </summary>
    public interface IAnimeMangaObject
    {
        #region Properties

        /// <summary>
        /// </summary>
        string Beschreibung { get; }

        /// <summary>
        /// </summary>
        Uri CoverUri { get; }

        /// <summary>
        /// </summary>
        string EnglischTitel { get; }

        /// <summary>
        /// </summary>
        Dictionary<string, Uri> Fsk { get; }

        /// <summary>
        /// </summary>
        string[] Genre { get; }

        /// <summary>
        /// </summary>
        Group[] Gruppen { get; }

        /// <summary>
        /// </summary>
        int Id { get; }

        /// <summary>
        /// </summary>
        Industry[] Industrie { get; }

        /// <summary>
        /// </summary>
        bool IstInitialisiert { get; }

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
        /// </summary>
        string[] Season { get; }

        /// <summary>
        /// </summary>
        AnimeMangaStatus Status { get; }

        /// <summary>
        /// </summary>
        string Synonym { get; }

        #endregion

        #region

        /// <summary>
        /// </summary>
        Task  Init();

        #endregion
    }
}