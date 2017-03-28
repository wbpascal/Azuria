using System;
using Azuria.Api.v1.Converters;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.User
{
    /// <summary>
    /// </summary>
    public class UserInfoDataModel : IDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("avatar")]
        public string AvatarId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("points_anime")]
        public int PointsAnime { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("points_forum")]
        public int PointsForum { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("points_info")]
        public int PointsInfo { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("points_manga")]
        public int PointsManga { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("points_misc")]
        public int PointsMisc { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("points_uploads")]
        public int PointsUploads { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("status_time")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime StatusLastChanged { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("status")]
        public string StatusText { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("uid")]
        public int UserId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        #endregion
    }
}