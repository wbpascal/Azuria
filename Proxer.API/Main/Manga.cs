using System;
using System.Collections.Generic;
using Proxer.API.Main.Minor;

namespace Proxer.API.Main
{
    /// <summary>
    /// </summary>
    public class Manga : IAnimeMangaObject
    {
        internal Manga(string name, int id)
        {
            this.ObjectType = AnimeMangaType.Manga;
            this.Name = name;
            this.Id = id;
        }

        public string Beschreibung { get; private set; }
        public string EnglischTitel { get; private set; }
        public Uri[] Fsk { get; private set; }
        public string[] Genre { get; private set; }
        public Group Gruppen { get; private set; }
        public int Id { get; private set; }
        public Industry[] Industrie { get; private set; }
        public string JapanTitel { get; private set; }
        public bool Lizensiert { get; private set; }
        public string Name { get; private set; }
        public AnimeMangaType ObjectType { get; private set; }
        public Dictionary<int, int> Season { get; private set; }
        public string Synonym { get; private set; }
    }
}