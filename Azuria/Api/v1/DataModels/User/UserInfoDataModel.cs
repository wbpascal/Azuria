using System;
using Azuria.Api.v1.Converters;
using Azuria.UserInfo;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.User
{
    internal class UserInfoDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("avatar")]
        internal string AvatarId { get; set; }

        internal UserPoints Points
            =>
            new UserPoints(this.PointsAnime, this.PointsManga, this.PointsInfo, this.PointsUploads, this.PointsForum,
                this.PointsMisc);

        [JsonProperty("points_anime")]
        internal int PointsAnime { get; set; }

        [JsonProperty("points_forum")]
        internal int PointsForum { get; set; }

        [JsonProperty("points_info")]
        internal int PointsInfo { get; set; }

        [JsonProperty("points_manga")]
        internal int PointsManga { get; set; }

        [JsonProperty("points_misc")]
        internal int PointsMisc { get; set; }

        [JsonProperty("points_uploads")]
        internal int PointsUploads { get; set; }

        internal UserStatus Status => new UserStatus(this.StatusText, this.StatusLastChanged);

        [JsonProperty("status_time")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        internal DateTime StatusLastChanged { get; set; }

        [JsonProperty("state")]
        internal string StatusText { get; set; }

        [JsonProperty("uid")]
        internal int UserId { get; set; }

        [JsonProperty("username")]
        internal string Username { get; set; }

        #endregion
    }
}