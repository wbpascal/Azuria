using System;
using System.Collections.Generic;
using Azuria.Api.v1.DataModels.Manga;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Manga
{
    internal class PagesConverter : DataConverter<PageDataModel[]>
    {
        /// <inheritdoc />
        public override PageDataModel[] ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            List<PageDataModel> lPageDataModels = new List<PageDataModel>();
            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
            {
                PageDataModel lPageDataModel = new PageDataModel();
                for (int innerIndex = 0; reader.Read() && reader.TokenType != JsonToken.EndArray; innerIndex++)
                    switch (innerIndex)
                    {
                        case 0:
                            lPageDataModel.ServerFileName = reader.Value.ToString();
                            break;
                        case 1:
                            lPageDataModel.PageHeight = Convert.ToInt32(reader.Value);
                            break;
                        case 2:
                            lPageDataModel.PageWidth = Convert.ToInt32(reader.Value);
                            break;
                    }
                lPageDataModels.Add(lPageDataModel);
            }

            return lPageDataModels.ToArray();
        }
    }
}