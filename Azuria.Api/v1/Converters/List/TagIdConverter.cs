using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.List
{
    /// <summary>
    /// 
    /// </summary>
    public class TagIdConverter : DataConverter<Tuple<int[], int[]>>
    {
        /// <inheritdoc />
        public override Tuple<int[], int[]> ConvertJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            int[] lTagIds = new int[0];
            int[] lNoTagIds = new int[0];

            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
            {
                switch (reader.Value?.ToString() ?? string.Empty)
                {
                    case "tags":
                        lTagIds = this.GetIdArray(reader).ToArray();
                        break;
                    case "notags":
                        lNoTagIds = this.GetIdArray(reader).ToArray();
                        break;
                }
            }

            return new Tuple<int[], int[]>(lTagIds, lNoTagIds);
        }

        private IEnumerable<int> GetIdArray(JsonReader reader)
        {
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray) yield break;
            while (reader.Read() && reader.TokenType != JsonToken.EndArray)
            {
                yield return Convert.ToInt32(reader.Value);
            }
        }
    }
}