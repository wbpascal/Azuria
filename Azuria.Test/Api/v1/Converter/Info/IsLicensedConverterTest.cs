using Azuria.Api.v1.Converters.Info;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.Converter.Info
{
    [TestFixture]
    public class IsLicensedConverterTest : DataConverterTestBase<bool?>
    {
        /// <inheritdoc />
        public IsLicensedConverterTest() : base(new IsLicensedConverter())
        {
        }

        [Test]
        public void CanConvertTest([Values(0, 1, 2)] int isLicensed)
        {
            bool? lValue = this.DeserializeValue(isLicensed.ToString());
            switch (isLicensed)
            {
                case 0:
                    Assert.Null(lValue);
                    break;
                case 1:
                    Assert.False(lValue);
                    break;
                case 2:
                    Assert.True(lValue);
                    break;
            }
        }
    }
}