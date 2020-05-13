using Azuria.Api.v1.Converters.Messenger;
using Azuria.Enums.Messenger;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.Converter.Conference
{
    public class MessageActionConverterTest : DataConverterTestBase<MessageAction>
    {
        /// <inheritdoc />
        public MessageActionConverterTest() : base(new MessageActionConverter())
        {
        }

        private void ConvertTest(string toConvert, MessageAction expected)
        {
            MessageAction lValue = this.DeserializeValue($"'{toConvert}'");
            Assert.AreEqual(expected, lValue);
        }

        [Test]
        public void AddUserTest()
        {
            this.ConvertTest("addUser", MessageAction.AddUser);
        }

        [Test]
        public void RemoveUserTest()
        {
            this.ConvertTest("removeUser", MessageAction.RemoveUser);
        }

        [Test]
        public void SetTopicTest()
        {
            this.ConvertTest("setTopic", MessageAction.SetTopic);
        }

        [Test]
        public void SetLeaderTest()
        {
            this.ConvertTest("setLeader", MessageAction.SetLeader);
        }

        [Test]
        public void DefaultValueTest()
        {
            this.ConvertTest("", MessageAction.NoAction);
        }
    }
}