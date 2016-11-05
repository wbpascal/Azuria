using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azuria.Api.v1.Converters;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.Utilities.Extensions;

// ReSharper disable StaticMemberInGenericType

namespace Azuria.Notifications.Media
{
    /// <summary>
    /// </summary>
    public sealed class MediaNotificationEnumerator<T> : IEnumerator<MediaNotification<T>>
        where T : IMediaObject
    {
        private static readonly Regex NotificationElementRegex = new Regex("<a class=\"notificationList\".*?<\\/a>");

        private static readonly Regex NotificationInfoRegex =
            new Regex(
                "<a class=\"notificationList\".*?notification(?<nid>[0-9]+)\".*?href=\"(?<link>(\\/watch|\\/chapter).*?)\\#top\">.*?\"nDate\">(?<ndate>.*?)<\\/div>",
                RegexOptions.ExplicitCapture);

        private readonly int _nodesToParse;
        private readonly Senpai _senpai;
        private MediaNotification<T>[] _content;
        private int _currentContentIndex = -1;

        internal MediaNotificationEnumerator(Senpai senpai, int nodesToParse = 0)
        {
            this._senpai = senpai;
            this._nodesToParse = nodesToParse;
        }

        #region Properties

        /// <inheritdoc />
        public MediaNotification<T> Current => this._content[this._currentContentIndex];

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
        private async Task<IProxerResult<IEnumerable<MediaNotification<T>>>> GetNextPage()
        {
            if (!this._senpai.IsProbablyLoggedIn)
                return new ProxerResult<IEnumerable<MediaNotification<T>>>(new NotLoggedInException(this._senpai));

            IProxerResult<string> lResponse = await this._senpai.HttpClient.GetRequest(
                new Uri("https://proxer.me/components/com_proxer/misc/notifications_misc.php"));
            if (!lResponse.Success || string.IsNullOrEmpty(lResponse.Result))
                return new ProxerResult<IEnumerable<MediaNotification<T>>>(lResponse.Exceptions);

            List<MediaNotification<T>> lNotifications = new List<MediaNotification<T>>();
            MatchCollection lMatches = NotificationElementRegex.Matches(lResponse.Result);

            foreach (Match lNotification in lMatches.OfType<Match>().Take(
                this._nodesToParse == 0 ? lMatches.Count : this._nodesToParse))
            {
                Match lInfo = NotificationInfoRegex.Match(lNotification.Value);
                if (!lInfo.Success) continue;
                lNotifications.AddIf(this.ParseNode(lInfo), notification => notification != null);
            }

            return new ProxerResult<IEnumerable<MediaNotification<T>>>(lNotifications);
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (this._content == null)
            {
                IProxerResult<IEnumerable<MediaNotification<T>>> lGetSearchResult =
                    Task.Run(this.GetNextPage).Result;
                if (!lGetSearchResult.Success || (lGetSearchResult.Result == null))
                    throw lGetSearchResult.Exceptions.FirstOrDefault() ?? new Exception("Unkown error");
                this._content = lGetSearchResult.Result as MediaNotification<T>[] ??
                                lGetSearchResult.Result.ToArray();
            }
            this._currentContentIndex++;
            return this._content.Length > this._currentContentIndex;
        }

        private MediaNotification<T> ParseNode(Match lNode)
        {
            int lNotificationId = Convert.ToInt32(lNode.Groups["nid"].Value);
            DateTime lDate = DateTime.ParseExact(lNode.Groups["ndate"].Value, "dd.MM.yyyy", CultureInfo.InvariantCulture);

            string[] lLinkInfo =
                lNode.Groups["link"].Value.Remove(0,
                    lNode.Groups["link"].Value.IndexOf("/", 1, StringComparison.Ordinal) + 1).Split('/');
            int lMediaId = Convert.ToInt32(lLinkInfo[0]);
            int lContentIndex = Convert.ToInt32(lLinkInfo[1]);
            MediaLanguage lLanguage = LanguageConverter.GetLanguageFromString(lLinkInfo[2]);

            IMediaObject lMediaObject = lNode.Groups["link"].Value.StartsWith("/watch")
                ? new Anime(lMediaId)
                : (IMediaObject) new Manga(lMediaId);

            return lMediaObject is T
                ? new MediaNotification<T>(lNotificationId, (T) lMediaObject, lContentIndex, lLanguage, lDate,
                    this._senpai)
                : null;
        }

        /// <inheritdoc />
        public void Reset()
        {
            this._content = new MediaNotification<T>[0];
            this._currentContentIndex = -1;
        }

        #endregion
    }
}