﻿using System;
using Azuria.AnimeManga;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    internal class SeasonDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("id")]
        internal int Id { get; set; }

        [JsonProperty("season")]
        internal Season Season { get; set; }

        [JsonProperty("year")]
        internal int Year { get; set; }

        #endregion

        #region

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            SeasonDataModel lDataModel = obj as SeasonDataModel;
            if (lDataModel != null) return lDataModel.Year == this.Year && lDataModel.Season == this.Season;
            return base.Equals(obj);
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return Convert.ToInt32(this.Year.ToString() + (int) this.Season);
        }

        #endregion
    }
}