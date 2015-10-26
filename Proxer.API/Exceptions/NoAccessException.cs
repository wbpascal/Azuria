using System;

namespace Proxer.API.Exceptions
{
    /// <summary>
    ///     Stellt eine Ausnahme dar, die ausgelöst wird, wenn der <see cref="Senpai">User</see> keine Berechtigung hat eine
    ///     Seite anzusehen.
    /// </summary>
    [Serializable]
    public class NoAccessException : Exception
    {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="NoAccessException" />-Klasse.
        /// </summary>
        public NoAccessException()
        {
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="NoAccessException" />-Klasse mit einer angegebenen Fehlermeldung.
        /// </summary>
        /// <param name="message">
        ///     Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird.
        ///     Kann bei dieser Klasse auch den Namen einer Methode darstellen, die diese Ausnahme ausgelöst hat.
        /// </param>
        public NoAccessException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="NoAccessException" />-Klasse mit einer
        ///     angegebenen Fehlermeldung und einem Verweis auf die innere Ausnahme, die diese Ausnahme verursacht hat.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird.</param>
        /// <param name="inner">
        ///     Die Ausnahme, die die aktuelle Ausnahme ausgelöst hat, oder ein Nullverweis (Nothing in Visual Basic),
        ///     wenn keine innere Ausnahme angegeben ist.
        /// </param>
        public NoAccessException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}