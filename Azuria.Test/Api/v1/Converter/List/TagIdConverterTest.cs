using System;
using Azuria.Api.v1.Converters.List;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.Converter.List
{
    public class TagIdConverterTest : DataConverterTestBase<Tuple<int[], int[]>>
    {
        /// <inheritdoc />
        public TagIdConverterTest() : base(new TagIdConverter())
        {
        }

        [Test]
        public void CanConvertTest()
        {
            Tuple<int[], int[]> lValue = this.DeserializeValue("{tags: [102, 31], notags:[278, 173, 12]}");
            Assert.AreEqual(new[] {102, 31}, lValue.Item1);
            Assert.AreEqual(new[] {278, 173, 12}, lValue.Item2);
        }
    }
}