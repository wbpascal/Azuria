﻿using System;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Main;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria
{
    /// <summary>
    ///     Eine Klasse, die alle Funktionen beherbergt, die Funktionen von Proxer.Me darstellen, aber nicht in andere Klassen
    ///     reinpassen.
    /// </summary>
    public static class ProxerClass
    {
        #region

        /// <summary>
        ///     Gibt ein Objekt zurück, dass einen Anime oder Manga
        ///     der spezifizierten ID repräsentiert.
        /// </summary>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <exception cref="ArgumentNullException">
        ///     Wird ausgelöst, wenn <paramref name="senpai" /> null (oder Nothing in Visual
        ///     Basic) ist.
        /// </exception>
        /// <param name="id">Die ID des <see cref="Main.Anime">Anime</see> oder <see cref="Main.Manga">Manga</see>.</param>
        /// <param name="senpai">Der Benutzer. (Muss eingeloggt sein)</param>
        /// <returns>Anime oder Manga der ID (Typecast erforderlich)</returns>
        [ItemNotNull]
        public static async Task<ProxerResult<IAnimeMangaObject>> GetAnimeMangaById(int id, [NotNull] Senpai senpai)
        {
            HtmlDocument lDocument = new HtmlDocument();

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling("https://proxer.me/info/" + id + "?format=raw",
                        senpai.LoginCookies, senpai.ErrHandler, senpai);

            if (!lResult.Success)
                return new ProxerResult<IAnimeMangaObject>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode lNode = lDocument.DocumentNode.ChildNodes[1].ChildNodes[1];
                if (lNode.InnerText.Equals("Episoden"))
                {
                    return
                        new ProxerResult<IAnimeMangaObject>(new Anime(
                            lDocument.DocumentNode
                                .ChildNodes[5].ChildNodes[2].FirstChild.ChildNodes[1].FirstChild.ChildNodes[1]
                                .ChildNodes[1].InnerText, id, senpai));
                }

                if (lNode.InnerText.Equals("Kapitel"))
                {
                    return
                        new ProxerResult<IAnimeMangaObject>(new Manga(
                            lDocument.DocumentNode.ChildNodes[5].ChildNodes[2].FirstChild.ChildNodes[1].FirstChild
                                .ChildNodes[1]
                                .ChildNodes[1].InnerText, id, senpai));
                }
            }
            catch
            {
                return
                    new ProxerResult<IAnimeMangaObject>(ErrorHandler.HandleError(senpai, lResponse).Exceptions);
            }

            return
                new ProxerResult<IAnimeMangaObject>(new Exception[] {new WrongResponseException {Response = lResponse}});
        }

        #endregion
    }
}