using System;

namespace Azuria.Exceptions
{
    /// <summary>
    /// </summary>
    public class CloudflareException : Exception
    {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="CloudflareException" />-Klasse.
        /// </summary>
        public CloudflareException()
        {
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="CloudflareException" />-Klasse mit einer angegebenen Fehlermeldung.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird.</param>
        public CloudflareException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="CloudflareException" />-Klasse mit einer
        ///     angegebenen Fehlermeldung und einem Verweis auf die innere Ausnahme, die diese Ausnahme verursacht hat.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird.</param>
        /// <param name="inner">
        ///     Die Ausnahme, die die aktuelle Ausnahme ausgelöst hat, oder ein Nullverweis (Nothing in Visual Basic),
        ///     wenn keine innere Ausnahme angegeben ist.
        /// </param>
        public CloudflareException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}