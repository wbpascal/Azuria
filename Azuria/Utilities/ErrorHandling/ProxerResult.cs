using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Azuria.Utilities.ErrorHandling
{
    /// <summary>
    ///     Eine Klasse, die ein Resultat einer Methode des API darstellt.
    /// </summary>
    /// <typeparam name="T">Der Typ des Resultats.</typeparam>
    public class ProxerResult<T> : ProxerResult
    {
        [UsedImplicitly]
        private ProxerResult()
        {
        }

        /// <summary>
        ///     Initialisiert die Klasse mit einem Resultat.
        /// </summary>
        /// <param name="result">Das Resultat</param>
        public ProxerResult([NotNull] T result)
        {
            this.Success = true;
            this.Result = result;
            this.Exceptions = new Exception[0];
        }

        /// <summary>
        ///     Initialisiert die Klasse mit Fehlermeldungen.
        /// </summary>
        /// <param name="exceptions">Die Fehlermeldungen.</param>
        public ProxerResult([NotNull] IEnumerable<Exception> exceptions) : base(exceptions)
        {
        }

        #region Properties

        /// <summary>
        ///     Gibt das Resultat zurück, das die Klasse repräsentiert, oder legt dieses fest.
        /// </summary>
        /// <value>Ist null, wenn <see cref="ProxerResult.Success" /> == false</value>
        [CanBeNull]
        public T Result { get; set; }

        #endregion

        #region

        /// <summary>
        ///     Eine Methode, die <paramref name="returnObject" /> zurückgibt, wenn <see cref="ProxerResult.Success" /> = false,
        ///     sonst wird das <see cref="Result">Resultat</see> zurückgegeben.
        /// </summary>
        /// <param name="returnObject">Das Objekt, dass zurückgegeben wird, wenn <see cref="ProxerResult.Success" /> = false.</param>
        /// <returns>Ein Objekt mit dem Typ <typeparamref name="T" /></returns>
        [CanBeNull]
        public T OnError([CanBeNull] T returnObject)
        {
            return this.Success ? this.Result : returnObject;
        }

        #endregion
    }

    /// <summary>
    ///     Eine Klasse, die ein Resultat einer Methode des API darstellt.
    /// </summary>
    public class ProxerResult
    {
        /// <summary>
        ///     Initialisiert die Klasse mit einem erfolgreichem Resultat.
        /// </summary>
        public ProxerResult()
        {
            this.Success = true;
            this.Exceptions = new Exception[0];
        }

        /// <summary>
        ///     Initialisiert die Klasse mit Fehlermeldungen.
        /// </summary>
        /// <param name="exceptions">Die Fehlermeldungen.</param>
        public ProxerResult([NotNull] IEnumerable<Exception> exceptions)
        {
            this.Success = false;
            this.Exceptions = exceptions;
        }

        #region Properties

        /// <summary>
        ///     Gibt die Fehler zurück, die während der Ausführung aufgetreten sind, oder legt diese fest.
        /// </summary>
        /// <value>Ist null, wenn <see cref="Success" /> == true</value>
        [NotNull]
        public IEnumerable<Exception> Exceptions { get; set; }

        /// <summary>
        ///     Gibt zurück, ob die Methode erfolg hatte, oder legt dieses fest.
        /// </summary>
        public bool Success { get; set; }

        #endregion

        #region

        /// <summary>
        ///     Fügt den <see cref="Exceptions">Ausnahmen</see> eine weiter hinzu.
        /// </summary>
        /// <param name="exception">Die Ausnahme die hinzugefügt werden soll.</param>
        public void AddException([NotNull] Exception exception)
        {
            List<Exception> lExceptions = this.Exceptions.ToList();
            lExceptions.Add(exception);
            this.Exceptions = lExceptions.ToArray();

            this.Success = false;
        }

        /// <summary>
        ///     Fügt den <see cref="Exceptions">Ausnahmen</see> weitere hinzu.
        /// </summary>
        /// <param name="exception">Die Ausnahme die hinzugefügt werden soll.</param>
        public void AddExceptions([NotNull] IEnumerable<Exception> exception)
        {
            List<Exception> lExceptions = this.Exceptions.ToList();
            lExceptions.AddRange(exception);
            this.Exceptions = lExceptions.ToArray();

            this.Success = false;
        }

        #endregion
    }
}