﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Newtonsoft.Json;

namespace Azuria.ErrorHandling
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
        ///     Fügt eine falsche Ausgabe hinzu.
        /// </summary>
        /// <param name="wrongHtml">Die falsche Ausgabe.</param>
        public void Add(string wrongHtml)
        {
            this.WrongHtml.Add(wrongHtml);
            this.Save();
        }

        internal static async Task<ProxerResult> HandleError(Senpai senpai, string wrongHtml, bool checkedLogin)
        {
            ProxerResult<bool> lCheckResult;
            if (!checkedLogin && (lCheckResult = await senpai.CheckLogin()).Success && !lCheckResult.Result)
            {
                return new ProxerResult(new Exception[] {new NotLoggedInException(senpai)});
            }

            senpai.ErrHandler.Add(wrongHtml);
            return new ProxerResult(new Exception[] {new WrongResponseException {Response = wrongHtml}});
        }

        internal static ProxerResult HandleError(Senpai senpai, string wrongHtml)
        {
            senpai.ErrHandler.Add(wrongHtml);
            return new ProxerResult(new Exception[] {new WrongResponseException {Response = wrongHtml}});
        }

        internal void Load()
        {
            if (true) //!string.IsNullOrEmpty(Settings.Default.errorHtml))
            {
                try
                {
                    this.WrongHtml = new List<string>();
                    //this.WrongHtml = JsonConvert.DeserializeObject<List<string>>(Settings.Default.errorHtml);
                }
                catch (JsonSerializationException)
                {
                    this.Reset();
                    this.Save();
                }
            }
            else
#pragma warning disable 162
            {
                this.Reset();
                this.Save();
            }
#pragma warning restore 162
        }

        /// <summary>
        ///     Erstellt eine neue Liste der Strings, die aussortiert werden sollen.
        /// </summary>
        public void Reset()
        {
            this.WrongHtml = new List<string>();
        }

        internal void Save()
        {
            //Settings.Default.errorHtml = JsonConvert.SerializeObject(this.WrongHtml);
            //Settings.Default.Save();
        }

        #endregion
    }
}