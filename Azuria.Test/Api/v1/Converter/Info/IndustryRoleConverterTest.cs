using Azuria.Api.v1.Converters;
using Azuria.Enums.Info;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.Converter.Info
{
    public class IndustryRoleConverterTest : DataConverterTestBase<IndustryRole>
    {
        /// <inheritdoc />
        public IndustryRoleConverterTest() : base(new IndustryRoleConverter())
        {
        }

        private void ConvertTest(string toConvert, IndustryRole expected)
        {
            IndustryRole lValue = this.DeserializeValue($"'{toConvert}'");
            Assert.AreEqual(expected, lValue);
        }

        [Test]
        public void StreamingRoleTest()
            => this.ConvertTest("streaming", IndustryRole.Streaming);

        [Test]
        public void RecordLabelRoleTest()
            => this.ConvertTest("record_label", IndustryRole.RecordLabel);

        [Test]
        public void TalentAgentRoleTest()
            => this.ConvertTest("talent_agent", IndustryRole.TalentAgent);

        [Test]
        public void PublisherRoleTest()
            => this.ConvertTest("publisher", IndustryRole.Publisher);

        [Test]
        public void StudioRoleTest()
            => this.ConvertTest("studio", IndustryRole.Studio);

        [Test]
        public void ProducerRoleTest()
            => this.ConvertTest("producer", IndustryRole.Producer);
    }
}