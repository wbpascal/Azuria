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

namespace Azuria.Notifications.OtherMedia
{
    /// <summary>
    /// </summary>
    internal sealed class OtherMediaNotificationEnumerator : IEnumerator<OtherMediaNotification>
    {
        private static readonly Regex NotificationInfoRegex = new Regex(
            "<a class=\"notificationList\".*?notification(?<nid>[0-9]+)\".*?href=\"(?<link>.*?)#top\">(?<message>.*?)<div.*?nDate\">(?<ndate>.*?)<\\/div>",
            RegexOptions.ExplicitCapture);

        private readonly Senpai _senpai;
        private OtherMediaNotification[] _content;
        private int _currentContentIndex = -1;

        internal OtherMediaNotificationEnumerator(Senpai senpai)
        {
            this._senpai = senpai;
        }

        #region Properties

        /// <inheritdoc />
        public OtherMediaNotification Current => this._content[this._currentContentIndex];

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
        private async Task<IProxerResult<IEnumerable<OtherMediaNotification>>> GetNextPage()
        {
            if (!this._senpai.IsProbablyLoggedIn)
                return new ProxerResult<IEnumerable<OtherMediaNotification>>(new NotLoggedInException(this._senpai));

            IProxerResult<string> lResponse = await this._senpai.HttpClient.GetRequest(
                new Uri("https://proxer.me/components/com_proxer/misc/notifications_misc.php"));
            if (!lResponse.Success || string.IsNullOrEmpty(lResponse.Result))
                return new ProxerResult<IEnumerable<OtherMediaNotification>>(lResponse.Exceptions);

            List<OtherMediaNotification> lNotifications = new List<OtherMediaNotification>();
            MatchCollection lMatches = NotificationInfoRegex.Matches(lResponse.Result.Replace("\n", ""));

            foreach (Match lNotification in lMatches)
                lNotifications.AddIf(
                    lNotification.Groups["link"].Value.ContainsOne("watch", "chapter")
                        ? this.ParseMediaNode(lNotification)
                        : this.ParseOtherNode(lNotification)
                    , notification => notification != null);

            return new ProxerResult<IEnumerable<OtherMediaNotification>>(lNotifications);
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (this._content == null)
            {
                IProxerResult<IEnumerable<OtherMediaNotification>> lGetSearchResult =
                    Task.Run(this.GetNextPage).Result;
                if (!lGetSearchResult.Success || (lGetSearchResult.Result == null))
                    throw lGetSearchResult.Exceptions.FirstOrDefault() ?? new Exception("Unkown error");
                this._content = lGetSearchResult.Result as OtherMediaNotification[] ??
                                lGetSearchResult.Result.ToArray();
            }
            this._currentContentIndex++;
            return this._content.Length > this._currentContentIndex;
        }

        private OtherMediaNotification ParseMediaNode(Match lNode)
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

            return new OtherMediaNotification(new MediaNotification(
                lNotificationId, lMediaObject, lContentIndex, lLanguage, lDate, this._senpai));
        }

        private OtherMediaNotification ParseOtherNode(Match node)
        {
            return new OtherMediaNotification(node.Groups["message"].Value, Convert.ToInt32(node.Groups["nid"].Value),
                this._senpai);
        }

        /// <inheritdoc />
        public void Reset()
        {
            this._content = new OtherMediaNotification[0];
            this._currentContentIndex = -1;
        }

        #endregion
    }
}