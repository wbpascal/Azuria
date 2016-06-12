using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Main;
using Azuria.Main.Minor;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Notifications.AnimeManga
{
    /// <summary>
    /// </summary>
    public class AnimeMangaNotificationEnumerator<T> : INotificationEnumerator<AnimeMangaNotification<T>>
        where T : class, IAnimeMangaObject
    {
        private readonly int _maxNotificationsCountToParse;
        private readonly Senpai _senpai;
        private int _itemIndex = -1;

        private AnimeMangaNotification<T>[] _notifications =
            new AnimeMangaNotification<T>[0];

        internal AnimeMangaNotificationEnumerator(Senpai senpai, int maxNotificationsCountToParse = -1)
        {
            this._senpai = senpai;
            this._maxNotificationsCountToParse = maxNotificationsCountToParse;
        }

        #region Geerbt

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            //nothing to do
        }

        /// <summary>Advances the enumerator to the next element of the collection.</summary>
        /// <returns>
        ///     true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
        ///     end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public bool MoveNext()
        {
            this._itemIndex++;
            if (this._notifications.Any()) return this._itemIndex < this._notifications.Length;

            Task<ProxerResult> lGetNotificationsTask = this.GetNotifications();
            lGetNotificationsTask.Wait();
            return lGetNotificationsTask.Result.Success && this._notifications.Any();
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public void Reset()
        {
            this._itemIndex = -1;
            this._notifications = new AnimeMangaNotification<T>[0];
        }

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        public AnimeMangaNotification<T> Current => this._notifications[this._itemIndex];

        /// <summary>Gets the current element in the collection.</summary>
        /// <returns>The current element in the collection.</returns>
        object IEnumerator.Current => this.Current;

        #endregion

        #region

        [ItemNotNull]
        private async Task<ProxerResult> GetNotifications()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/components/com_proxer/misc/notifications_misc.php"),
                        this._senpai.LoginCookies,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode[] lNodes =
                    lDocument.DocumentNode.SelectNodesUtility("class", "notificationList").ToArray();

                List<AnimeMangaNotification<T>> lAnimeMangaUpdateObjects =
                    new List<AnimeMangaNotification<T>>();

                int lNotificationsParsed = 0;
                foreach (HtmlNode curNode in lNodes.Where(curNode => curNode.InnerText.StartsWith("Lesezeichen:")))
                {
                    if (this._maxNotificationsCountToParse != -1 &&
                        lNotificationsParsed >= this._maxNotificationsCountToParse) break;

                    string lName;
                    int lNumber;
                    Type lContentParentType = curNode.GetAttributeValue("href", "").StartsWith("/watch")
                        ? typeof(Anime)
                        : curNode.GetAttributeValue("href", "").StartsWith("/chapter") ? typeof(Manga) : null;

                    if (lContentParentType == null) continue;

                    int lAnimeMangaId = Convert.ToInt32(curNode.GetAttributeValue("href", "/watch/-1/").Split('/')[2]);
                    string lMessage = curNode.ChildNodes["u"].InnerText;

                    #region Language
                    AnimeLanguage lAnimeLanguage = AnimeLanguage.Unknown;
                    Language lLanguage = Language.Unkown;
                    if (lContentParentType == typeof(Anime))
                    {
                        switch (
                            curNode.GetAttributeValue("href", "/watch/-1/error#top")
                                .Split('/')
                                .Last()
                                .Split('#')
                                .First())
                        {
                            case "engsub":
                                lAnimeLanguage = AnimeLanguage.EngSub;
                                break;
                            case "engdub":
                                lAnimeLanguage = AnimeLanguage.EngDub;
                                break;
                            case "gersub":
                                lAnimeLanguage = AnimeLanguage.GerSub;
                                break;
                            case "gerdub":
                                lAnimeLanguage = AnimeLanguage.GerDub;
                                break;
                        }
                    }else if (lContentParentType == typeof(Manga))
                    {
                        switch (curNode.GetAttributeValue("href", "/watch/-1/error#top")
                                .Split('/')
                                .Last()
                                .Split('#')
                                .First())
                        {
                            case "en":
                                lLanguage = Language.English;
                                break;
                            case "de":
                                lLanguage = Language.German;
                                break;
                        }
                    }
                    #endregion

                    if (lMessage.IndexOf('#') != -1)
                    {
                        lName = lMessage.Split('#')[0].Trim();
                        if (!int.TryParse(lMessage.Split('#')[1], out lNumber)) lNumber = -1;
                    }
                    else
                    {
                        lName = "";
                        lNumber = -1;
                    }

                    if (typeof(T) == typeof(Anime) && lContentParentType == typeof(Anime))
                        lAnimeMangaUpdateObjects.AddIf(
                            new AnimeMangaNotification<Anime>(
                                new Anime.Episode(new Anime(lName, lAnimeMangaId, this._senpai), lNumber, lAnimeLanguage,
                                    this._senpai), this._senpai) as AnimeMangaNotification<T>,
                            notification => notification != null);
                    else if (lContentParentType == typeof(Manga))
                        lAnimeMangaUpdateObjects.AddIf(
                            new AnimeMangaNotification<Manga>(
                                new Manga.Chapter(new Manga(lName, lAnimeMangaId, this._senpai),
                                    lNumber, lLanguage, this._senpai), this._senpai) as AnimeMangaNotification<T>,
                            notification => notification != null);

                    lNotificationsParsed++;
                }

                this._notifications = lAnimeMangaUpdateObjects.ToArray();

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        #endregion
    }
}