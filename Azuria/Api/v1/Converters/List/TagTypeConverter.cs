﻿using System;
using System.Collections.Generic;
using Azuria.Enums.List;
using Azuria.Helpers;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.List
{
    internal class TagTypeConverter : DataConverter<TagType>
    {
        #region Methods

        public override TagType ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string lTagType = reader.Value?.ToString();
            Dictionary<string, TagType> lStringDictionary = EnumHelpers.GetDescriptionDictionary<TagType>();
            return lStringDictionary.ContainsKey(lTagType) ? lStringDictionary[lTagType] : TagType.Unkown;
        }

        #endregion
    }
}