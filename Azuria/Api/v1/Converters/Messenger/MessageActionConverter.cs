using System;
using Azuria.Enums.Messenger;
using Newtonsoft.Json;

namespace Azuria.Api.v1.Converters.Messenger
{
    internal class MessageActionConverter : DataConverter<MessageAction>
    {
        #region Methods

        /// <inheritdoc />
        public override MessageAction ConvertJson(
            JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
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

        #endregion
    }
}