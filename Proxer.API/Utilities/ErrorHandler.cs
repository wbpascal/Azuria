using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorHandler
    {
        /// <summary>
        /// 
        /// </summary>
        internal ErrorHandler()
        {
            this.load();
        }

        /// <summary>
        /// 
        /// </summary>
        internal List<string> WrongHtml { get; private set; }

        /// <summary>
        /// Erstellt eine neue Liste der strings, die aussortiert werden sollen
        /// </summary>
        public void reset()
        {
            this.WrongHtml = new List<string>();
        }
        /// <summary>
        /// Läd die Liste aus den Einstellungen
        /// </summary>
        internal void load()
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.errorHtml))
            {
                try
                {
                    this.WrongHtml = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(Properties.Settings.Default.errorHtml);
                }
                catch (Newtonsoft.Json.JsonSerializationException)
                {
                    this.reset();
                    this.save();
                }
            }
            else
            {
                this.reset();
                this.save();
            }
        }
        /// <summary>
        /// Speichert die Liste in den Einstellungen
        /// </summary>
        internal void save()
        {
            Properties.Settings.Default.errorHtml = Newtonsoft.Json.JsonConvert.SerializeObject(this.WrongHtml);
            Properties.Settings.Default.Save();
        }
        /// <summary>
        /// Hinzufügen einer falschen Ausgabe
        /// </summary>
        /// <param name="wrongHtml"></param>
        internal void add(string wrongHtml)
        {
            this.WrongHtml.Add(wrongHtml);
            this.save();
        }
    }
}
