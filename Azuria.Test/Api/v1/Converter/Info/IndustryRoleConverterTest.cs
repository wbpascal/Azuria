using Azuria.Api.v1.Converter;
using Azuria.Enums.Info;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.Converter.Info
{
    public class IndustryRoleConverterTest : DataConverterTestBase<IndustryType>
    {
        /// <inheritdoc />
        public IndustryRoleConverterTest() : base(new IndustryRoleConverter())
        {
        }

        private void ConvertTest(string toConvert, IndustryType expected)
        {
            IndustryType lValue = this.DeserializeValue($"'{toConvert}'");
            Assert.AreEqual(expected, lValue);
        }

        [Test]
        public void StreamingRoleTest()
        {
            this.ConvertTest("streaming", IndustryType.Streaming);
        }

        [Test]
        public void RecordLabelRoleTest()
        {
            this.ConvertTest("record_label", IndustryType.RecordLabel);
        }

        [Test]
        public void TalentAgentRoleTest()
        {
            this.ConvertTest("talent_agent", IndustryType.TalentAgent);
        }

        [Test]
        public void PublisherRoleTest()
        {
            this.ConvertTest("publisher", IndustryType.Publisher);
        }

        [Test]
        public void StudioRoleTest()
        {
            this.ConvertTest("studio", IndustryType.Studio);
        }

        [Test]
        public void ProducerRoleTest()
        {
            this.ConvertTest("producer", IndustryType.Producer);
        }
    }
}