using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API
{
    /// <summary>
    /// Fasst alle Benachrichtigungen-Klassen in ein Interface zusammen.
    /// Wird für die Events benutzt
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Anzahl der Updates in der Benachrichtigung
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gibt den Inhalt der Benachrichtigung zurück
        /// </summary>
        string[] getUpdates();
    }
}
