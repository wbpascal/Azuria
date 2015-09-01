using System.Collections.Generic;
using Newtonsoft.Json;
using Proxer.API.Properties;

namespace Proxer.API.Utilities
{
    /// <summary>
    /// </summary>
    public class ErrorHandler
    {
        /// <summary>
        /// </summary>
        internal ErrorHandler()
        {
            this.Load();
        }

        #region Properties

        /// <summary>
        /// </summary>
        internal List<string> WrongHtml { get; private set; }

        #endregion

        #region

        /// <summary>
        ///     Erstellt eine neue Liste der strings, die aussortiert werden sollen
        /// </summary>
        public void Reset()
        {
            this.WrongHtml = new List<string>();
        }

        /// <summary>
        ///     Läd die Liste aus den Einstellungen
        /// </summary>
        internal void Load()
        {
            if (!string.IsNullOrEmpty(Settings.Default.errorHtml))
            {
                try
                {
                    this.WrongHtml = JsonConvert.DeserializeObject<List<string>>(Settings.Default.errorHtml);
                }
                catch (JsonSerializationException)
                {
                    this.Reset();
                    this.Save();
                }
            }
            else
            {
                this.Reset();
                this.Save();
            }
        }

        /// <summary>
        ///     Speichert die Liste in den Einstellungen
        /// </summary>
        internal void Save()
        {
            Settings.Default.errorHtml = JsonConvert.SerializeObject(this.WrongHtml);
            Settings.Default.Save();
        }

        /// <summary>
        ///     Hinzufügen einer falschen Ausgabe
        /// </summary>
        /// <param name="wrongHtml"></param>
        internal void Add(string wrongHtml)
        {
            this.WrongHtml.Add(wrongHtml);
            this.Save();
        }

        #endregion
    }
}