using System;

namespace Azuria.Exceptions
{
    /// <summary>
    ///     Stellt einen Fehler da, der ausgelöst wird, wenn eine Eigenschaft aufgerufen wird, die das Initialisieren des
    ///     Objektes erfordert.
    /// </summary>
    [Serializable]
    public class InitializeNeededException : Exception
    {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="InitializeNeededException" />-Klasse.
        /// </summary>
        public InitializeNeededException()
        {
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="InitializeNeededException" />-Klasse mit einer angegebenen
        ///     Fehlermeldung.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird.</param>
        public InitializeNeededException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="InitializeNeededException" />-Klasse mit einer
        ///     angegebenen Fehlermeldung und einem Verweis auf die innere Ausnahme, die diese Ausnahme verursacht hat.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird.</param>
        /// <param name="inner">
        ///     Die Ausnahme, die die aktuelle Ausnahme ausgelöst hat, oder ein Nullverweis (Nothing in Visual Basic),
        ///     wenn keine innere Ausnahme angegeben ist.
        /// </param>
        public InitializeNeededException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}