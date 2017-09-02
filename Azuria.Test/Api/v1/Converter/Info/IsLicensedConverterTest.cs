using Azuria.Api.v1.Converters.Info;
using Xunit;

namespace Azuria.Test.Api.v1.Converter.Info
{
    public class IsLicensedConverterTest : DataConverterTestBase<bool?>
    {
        /// <inheritdoc />
        public IsLicensedConverterTest() : base(new IsLicensedConverter())
        {
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void CanConvertTest(int isLicensed)
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