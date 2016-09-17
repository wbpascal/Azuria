using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azuria.Api;
using Azuria.Api.v1.Converters;
using Azuria.ErrorHandling;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.Utilities.Extensions;

// ReSharper disable StaticMemberInGenericType

namespace Azuria.Notifications.AnimeManga
{
    /// <summary>
    /// </summary>
    public sealed class AnimeMangaNotificationEnumerator<T> : IEnumerator<AnimeMangaNotification<T>>
        where T : IAnimeMangaObject
    {
        private static readonly Regex NotificationElementRegex = new Regex("<a class=\"notificationList\".*?<\\/a>");

        private static readonly Regex NotificationInfoRegex =
            new Regex(
                "<a class=\"notificationList\".*?notification(?<nid>[0-9]+)\".*?href=\"(?<link>(\\/watch|\\/chapter).*?)\\#top\">.*?\"nDate\">(?<ndate>.*?)<\\/div>",
                RegexOptions.ExplicitCapture);

        private readonly int _nodesToParse;
        private readonly Senpai _senpai;
        private AnimeMangaNotification<T>[] _content;
        private int _currentContentIndex = -1;

        internal AnimeMangaNotificationEnumerator(Senpai senpai, int nodesToParse = 0)
        {
            this._senpai = senpai;
            this._nodesToParse = nodesToParse;
        }

        #region Properties

        /// <inheritdoc />
        public AnimeMangaNotification<T> Current => this._content[this._currentContentIndex];

        /// <inheritdoc />
        object IEnumerator.Current => this.Current;

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Dispose()
        {
            this._content = null;
        }

        /// <inheritdoc />
        private async Task<ProxerResult<IEnumerable<AnimeMangaNotification<T>>>> GetNextPage()
        {
            ProxerResult<string> lResponse =
                await
                    ApiInfo.HttpClient.GetRequest(
                        new Uri("https://proxer.me/components/com_proxer/misc/notifications_misc.php"), this._senpai);
            if (!lResponse.Success || string.IsNullOrEmpty(lResponse.Result))
                return new ProxerResult<IEnumerable<AnimeMangaNotification<T>>>(lResponse.Exceptions);

            List<AnimeMangaNotification<T>> lNotifications = new List<AnimeMangaNotification<T>>();
            MatchCollection lMatches = NotificationElementRegex.Matches(lResponse.Result);

            foreach (
                Match lNotification in
                lMatches.OfType<Match>().Take(this._nodesToParse == 0 ? lMatches.Count : this._nodesToParse))
            {
                Match lInfo = NotificationInfoRegex.Match(lNotification.Value);
                if (!lInfo.Success) continue;
                lNotifications.AddIf(this.ParseNode(lInfo), notification => notification != null);
            }

            return new ProxerResult<IEnumerable<AnimeMangaNotification<T>>>(lNotifications);
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (this._content == null)
            {
                ProxerResult<IEnumerable<AnimeMangaNotification<T>>> lGetSearchResult =
                    Task.Run(this.GetNextPage).Result;
                if (!lGetSearchResult.Success || (lGetSearchResult.Result == null))
                    throw lGetSearchResult.Exceptions.FirstOrDefault() ?? new Exception("Unkown error");
                this._content = lGetSearchResult.Result as AnimeMangaNotification<T>[] ??
                                lGetSearchResult.Result.ToArray();
            }
            this._currentContentIndex++;
            return this._content.Length > this._currentContentIndex;
        }

        private AnimeMangaNotification<T> ParseNode(Match lNode)
        {
            int lNotificationId = Convert.ToInt32(lNode.Groups["nid"].Value);
            DateTime lDate = DateTime.ParseExact(lNode.Groups["ndate"].Value, "dd.MM.yyyy", CultureInfo.InvariantCulture);

            string[] lLinkInfo =
                lNode.Groups["link"].Value.Remove(0,
                    lNode.Groups["link"].Value.IndexOf("/", 1, StringComparison.Ordinal) + 1).Split('/');
            int lAnimeMangaId = Convert.ToInt32(lLinkInfo[0]);
            int lContentIndex = Convert.ToInt32(lLinkInfo[1]);
            AnimeMangaLanguage lLanguage = LanguageConverter.GetLanguageFromString(lLinkInfo[2]);

            IAnimeMangaObject lAnimeMangaObject = lNode.Groups["link"].Value.StartsWith("/watch")
                ? new Anime(lAnimeMangaId)
                : (IAnimeMangaObject) new Manga(lAnimeMangaId);

            return lAnimeMangaObject is T
                ? new AnimeMangaNotification<T>(lNotificationId, (T) lAnimeMangaObject, lContentIndex, lLanguage, lDate,
                    this._senpai)
                : null;
        }

        /// <inheritdoc />
        public void Reset()
        {
            this._content = new AnimeMangaNotification<T>[0];
            this._currentContentIndex = -1;
        }

        #endregion
    }
}