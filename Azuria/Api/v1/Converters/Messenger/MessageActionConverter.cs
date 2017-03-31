using System;
using Azuria.Enums.Messenger;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Messenger
{
    internal class MessageActionConverter : JsonConverter
    {
        #region Methods

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MessageAction);
        }

        public override object ReadJson(
            JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            switch (reader.Value.ToString())
            {
                case "addUser":
                    return MessageAction.AddUser;
                case "removeUser":
                    return MessageAction.RemoveUser;
                case "setTopic":
                    return MessageAction.SetTopic;
                case "setLeader":
                    return MessageAction.SetLeader;
                default:
                    return MessageAction.NoAction;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}