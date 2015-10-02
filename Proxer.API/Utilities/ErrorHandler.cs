using System.Collections.Generic;
using Newtonsoft.Json;
using Proxer.API.Properties;

namespace Proxer.API.Utilities
{
    /// <summary>
    ///     Fehler in HTML-Dateien werden hier frühzeitig erkannt
    /// </summary>
    public class ErrorHandler
    {
        internal ErrorHandler()
        {
            this.Load();
        }

        #region Properties

        internal List<string> WrongHtml { get; private set; }

        #endregion

        #region

        /// <summary>
        ///     Erstellt eine neue Liste der Strings, die aussortiert werden sollen
        /// </summary>
        public void Reset()
        {
            this.WrongHtml = new List<string>();
        }

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

        internal void Save()
        {
            Settings.Default.errorHtml = JsonConvert.SerializeObject(this.WrongHtml);
            Settings.Default.Save();
        }

        /// <summary>
        ///     Fügt eine falsche Ausgabe hinzu.
        /// </summary>
        /// <param name="wrongHtml">Die falsche Ausgabe.</param>
        public void Add(string wrongHtml)
        {
            this.WrongHtml.Add(wrongHtml);
            this.Save();
        }

        #endregion
    }
}