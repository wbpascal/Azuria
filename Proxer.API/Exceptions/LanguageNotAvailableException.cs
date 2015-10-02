using System;

namespace Proxer.API.Exceptions
{
    /// <summary>
    ///     Stellt einen Fehler da, der ausgelöst wird, wenn ein Anime oder Manga in einer Sprache abgerufen wird, in der es
    ///     nicht verfügbar ist.
    /// </summary>
    [Serializable]
    public class LanguageNotAvailableException : Exception
    {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="LanguageNotAvailableException" />-Klasse.
        /// </summary>
        public LanguageNotAvailableException()
        {
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="LanguageNotAvailableException" />-Klasse mit einer angegebenen
        ///     Fehlermeldung.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird.</param>
        public LanguageNotAvailableException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="LanguageNotAvailableException" />-Klasse mit einer
        ///     angegebenen Fehlermeldung und einem Verweis auf die innere Ausnahme, die diese Ausnahme verursacht hat.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird.</param>
        /// <param name="inner">
        ///     Die Ausnahme, die die aktuelle Ausnahme ausgelöst hat, oder ein Nullverweis (Nothing in Visual Basic),
        ///     wenn keine innere Ausnahme angegeben ist.
        /// </param>
        public LanguageNotAvailableException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}