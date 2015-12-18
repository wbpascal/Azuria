using System;
using System.Net;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Main;
using Azuria.Utilities;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using RestSharp;

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
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="ArgumentNullException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn <paramref name="senpai" /> null (oder Nothing in Visual Basic) ist.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="id">Die ID des <see cref="Main.Anime">Anime</see> oder <see cref="Main.Manga">Manga</see>.</param>
        /// <param name="senpai">Der Benutzer. (Muss eingeloggt sein)</param>
        /// <returns>Anime oder Manga der ID (Typecast erforderlich)</returns>
        public static async Task<ProxerResult<IAnimeMangaObject>> GetAnimeManga(int id, Senpai senpai)
        {
            if (senpai == null)
                return new ProxerResult<IAnimeMangaObject>(new Exception[] {new ArgumentNullException(nameof(senpai))});

            HtmlDocument lDocument = new HtmlDocument();
            string lResponse;

            IRestResponse lResponseObject =
                await
                    HttpUtility.GetWebRequestResponse("https://proxer.me/info/" + id + "?format=raw",
                        senpai.LoginCookies);
            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseObject.Content))
                lResponse = System.Web.HttpUtility.HtmlDecode(lResponseObject.Content).Replace("\n", "");
            else
                return
                    new ProxerResult<IAnimeMangaObject>(new[]
                    {new WrongResponseException(), lResponseObject.ErrorException});

            if (string.IsNullOrEmpty(lResponse) || !Utility.CheckForCorrectResponse(lResponse, senpai.ErrHandler))
                return
                    new ProxerResult<IAnimeMangaObject>(new Exception[]
                    {new WrongResponseException {Response = lResponse}});

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