using System;

namespace Proxer.API.Exceptions
{
    /// <summary>
    ///     Stellt einen Fehler da, der ausgelöst wird, wenn die Antwort des Servers bei einer Anfrage nicht mit der Erwarteten
    ///     übereinstimmt.
    /// </summary>
    [Serializable]
    public class WrongResponseException : Exception
    {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="WrongResponseException" />-Klasse.
        /// </summary>
        public WrongResponseException()
        {
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="WrongResponseException" />-Klasse mit einer angegebenen
        ///     Fehlermeldung.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird.</param>
        public WrongResponseException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="WrongResponseException" />-Klasse mit einer
        ///     angegebenen Fehlermeldung und einem Verweis auf die innere Ausnahme, die diese Ausnahme verursacht hat.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird.</param>
        /// <param name="inner">
        ///     Die Ausnahme, die die aktuelle Ausnahme ausgelöst hat, oder ein Nullverweis (Nothing in Visual Basic),
        ///     wenn keine innere Ausnahme angegeben ist.
        /// </param>
        public WrongResponseException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}