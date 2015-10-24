using System;

namespace Proxer.API.Exceptions
{
    /// <summary>
    ///     Stellte einen Fehler da, der ausgelöst wird, wenn Proxer das ausfüllen eines Captchas fordert.
    /// </summary>
    public class CaptchaException : Exception
    {
        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="CaptchaException" />-Klasse.
        /// </summary>
        public CaptchaException()
        {
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="CaptchaException" />-Klasse mit einer angegebenen Fehlermeldung.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird.</param>
        public CaptchaException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="CaptchaException" />-Klasse mit einer
        ///     angegebenen Fehlermeldung und einem Verweis auf die innere Ausnahme, die diese Ausnahme verursacht hat.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird.</param>
        /// <param name="inner">
        ///     Die Ausnahme, die die aktuelle Ausnahme ausgelöst hat, oder ein Nullverweis (Nothing in Visual Basic),
        ///     wenn keine innere Ausnahme angegeben ist.
        /// </param>
        public CaptchaException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}